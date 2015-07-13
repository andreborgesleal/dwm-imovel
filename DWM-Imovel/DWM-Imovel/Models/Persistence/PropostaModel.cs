using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using DWM.Models.Entidades;
using DWM.Models.Repositories;

namespace DWM.Models.Persistence
{
    public class PropostaModel : CrudModel<Proposta, PropostaViewModel, ApplicationContext>
    {
        #region Constructor
        public PropostaModel() { }
        public PropostaModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override PropostaViewModel AfterInsert(PropostaViewModel value)
        {
            #region Verifica se tem Etapa específica para o empreendimento. Se não tiver, trás a etapa "Proposta" padrão para todos os empreendimentos
            int? _etapaId = db.Etapas.Where(info => info.empreendimentoId == value.empreendimentoId && info.descricao == "Proposta").FirstOrDefault().etapaId;

            if (!_etapaId.HasValue)
                _etapaId = db.Etapas.Where(info => info.descricao == "Proposta").FirstOrDefault().etapaId;
            #endregion

            EsteiraViewModel esteiraViewModel = new EsteiraViewModel()
            {
                propostaId = value.propostaId,
                dt_evento = Funcoes.Brasilia(),
                etapaId = _etapaId.Value,
                dt_ocorrencia = value.dt_proposta,
                usuarioId = value.usuarioId,
                nome = value.nome,
                login = value.login
            };

            EsteiraModel esteiraModel = new EsteiraModel();
            esteiraModel.Create(this.db, this.seguranca_db);

            esteiraViewModel = esteiraModel.Insert(esteiraViewModel);

            value.mensagem = esteiraViewModel.mensagem;

            return value;
        }
        public override Proposta MapToEntity(PropostaViewModel value)
        {
            Proposta proposta = new Proposta()
            {
                propostaId = value.propostaId,
                empreendimentoId = value.empreendimentoId,
                clienteId = value.clienteId,
                dt_proposta = value.dt_proposta,
                unidade = value.unidade,
                torre = value.torre,
                valor = value.valor,
                vr_comissao = value.vr_comissao,
                etapaId = value.etapaId,
                dt_ultimo_status = value.dt_ultimo_status,
                operacaoId = value.operacaoId,
                corretor1Id = value.corretor1Id,
                corretor2Id = value.corretor2Id,
                usuarioId = value.usuarioId,
                nome = value.nome,
                login = value.login,
            };

            return proposta;
        }

        public override PropostaViewModel MapToRepository(Proposta entity)
        {
            return new PropostaViewModel()
            {
                propostaId = entity.propostaId,
                empreendimentoId = entity.empreendimentoId,
                descricao_empreendimento = db.Empreendimentos.Find(entity.empreendimentoId).nomeEmpreend,
                clienteId = entity.clienteId,
                nome_cliente = db.Clientes.Find(entity.clienteId).nome,
                dt_proposta = entity.dt_proposta,
                unidade = entity.unidade,
                torre = entity.torre,
                valor = entity.valor,
                vr_comissao = entity.vr_comissao,
                etapaId = entity.etapaId,
                dt_ultimo_status = entity.dt_ultimo_status,
                operacaoId = entity.operacaoId,
                corretor1Id = entity.corretor1Id,
                nome_corretor1 = entity.corretor1Id.HasValue ? db.Corretores.Find(entity.corretor1Id).nome : "",
                corretor2Id = entity.corretor2Id,
                nome_corretor2 = entity.corretor2Id.HasValue ? db.Corretores.Find(entity.corretor2Id).nome : "",
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Proposta Find(PropostaViewModel key)
        {
            return db.Propostas.Find(key.propostaId);
        }

        public override Validate Validate(PropostaViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.empreendimentoId == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Emprendimento").ToString();
                value.mensagem.MessageBase = "Empreendimento deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.clienteId == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Cliente").ToString();
                value.mensagem.MessageBase = "Cliente deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.valor == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Cliente").ToString();
                value.mensagem.MessageBase = "Valor deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.vr_comissao == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Cliente").ToString();
                value.mensagem.MessageBase = "Valor da comissão deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.etapaId == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Cliente").ToString();
                value.mensagem.MessageBase = "Etapa deve ser preenchida";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.dt_ultimo_status == null || value.dt_ultimo_status == DateTime.MinValue)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Cliente").ToString();
                value.mensagem.MessageBase = "Valor deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            
            return value.mensagem;
        }
        #endregion
    }

    public class ListViewProposta : ListViewModel<PropostaViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewProposta() { }
        public ListViewProposta(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<PropostaViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param.Count() > 0 && param[0] != null ? param[0].ToString() : null;
            return (from c in db.Propostas
                    where (_nome == null || String.IsNullOrEmpty(_nome) || c.torre.StartsWith(_nome.Trim()))
                    orderby c.torre
                    select new PropostaViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        clienteId = c.clienteId,
                        propostaId = c.propostaId,
                        empreendimentoId = c.empreendimentoId,
                        dt_proposta = c.dt_proposta,
                        unidade = c.unidade,
                        torre = c.torre,
                        valor = c.valor,
                        vr_comissao = c.vr_comissao,
                        etapaId = c.etapaId,
                        dt_ultimo_status = c.dt_ultimo_status,
                        operacaoId = c.operacaoId,
                        PageSize = pageSize,
                        TotalCount = (from c1 in db.Propostas
                                      where (_nome == null || String.IsNullOrEmpty(_nome) || c1.torre.StartsWith(_nome.Trim()))
                                      select c1).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new PropostaModel().getObject((PropostaViewModel)id);
        }
        #endregion
    }

}