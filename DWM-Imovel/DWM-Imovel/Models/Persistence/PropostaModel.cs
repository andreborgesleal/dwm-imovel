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
using App_Dominio.Security;

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


        public override PropostaViewModel BeforeInsert(PropostaViewModel value)
        {
            value.dt_ultimo_status = value.dt_proposta;
            value.etapaId = 0;
            value.situacao = "A";
            return value;
        }

        #region Métodos da classe CrudModel
        public override PropostaViewModel AfterInsert(PropostaViewModel value)
        {

            #region Verifica se tem Etapa específica para o empreendimento. Se não tiver, trás a etapa "Proposta" padrão para todos os empreendimentos
            int _etapaId;
            if (db.Etapas.Where(info => info.empreendimentoId == value.empreendimentoId && info.descricao == "Proposta").Count() > 0)
                _etapaId = db.Etapas.Where(info => info.empreendimentoId == value.empreendimentoId && info.descricao == "Proposta").FirstOrDefault().etapaId;
            else
                _etapaId = db.Etapas.Where(info => info.descricao == "Proposta").FirstOrDefault().etapaId;
            #endregion

            EsteiraViewModel esteiraViewModel = new EsteiraViewModel()
            {
                propostaId = value.propostaId,
                dt_evento = Funcoes.Brasilia(),
                etapaId = _etapaId,
                dt_ocorrencia = value.dt_proposta,
                observacao = "Inclusão de proposta. Cliente: " + db.Clientes.Find(value.clienteId).nome,
                usuarioId = value.usuarioId,
                nome = value.nome,
                login = value.login,
                uri = "Propostas/Create"
            };

            EsteiraModel esteiraModel = new EsteiraModel();
            esteiraModel.Create(this.db, this.seguranca_db);

            esteiraViewModel = esteiraModel.Insert(esteiraViewModel);

            value.mensagem = esteiraViewModel.mensagem;

            return value;
        }

        public override Proposta MapToEntity(PropostaViewModel value)
        {
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            Usuario u = security._getUsuarioById(value.usuarioId, seguranca_db);

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
                nome = u.nome,
                login = u.login,
                situacao = value.situacao,
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
                descricao_etapa = db.Etapas.Find(entity.etapaId).descricao,
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
                usuarioId = entity.usuarioId,
                nome = entity.nome,
                login = entity.login,
                situacao = entity.situacao,
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

            if (value.empreendimentoId < 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Emprendimento").ToString();
                value.mensagem.MessageBase = "Empreendimento deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.clienteId < 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Cliente").ToString();
                value.mensagem.MessageBase = "Cliente deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.torre.Trim() == "")
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Torre").ToString();
                value.mensagem.MessageBase = "Torre deve ser preenchida";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.unidade.Trim() == "")
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Unidade").ToString();
                value.mensagem.MessageBase = "Unidade deve ser preenchida";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #region Verifica se a torre e unidade já não foram incluídas para o mesmo empreendimento
            if (operation == Crud.INCLUIR)
            {
                if (db.Propostas.Where(info => info.empreendimentoId == value.empreendimentoId &&
                                                info.torre == value.torre &&
                                                info.unidade == value.unidade).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "Já existe uma Torre e Unidade cadastrada para este empreendimento";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            else if (operation == Crud.ALTERAR)
            {
                if (db.Propostas.Where(info => info.propostaId != value.propostaId &&
                                                info.empreendimentoId == value.empreendimentoId &&
                                                info.torre == value.torre &&
                                                info.unidade == value.unidade).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "Já existe uma Torre e Unidade cadastrada para este empreendimento";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            #endregion

            if (value.valor <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Valor geral da venda").ToString();
                value.mensagem.MessageBase = "Valor geral da venda deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.vr_comissao <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Valor da Comissão").ToString();
                value.mensagem.MessageBase = "Valor da comissão deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.etapaId < 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Etapa").ToString();
                value.mensagem.MessageBase = "Etapa deve ser preenchida";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #region Não permite que o valor da proposta seja alterado, caso a proposta não esteja na etapa Proposta Analise e Reanalise
            //string etapa = db.Etapas.Find(value.etapaId).descricao;
            //if (etapa != "Proposta" && etapa != "Análise Inicial" && etapa != "Reanálise")
            //{
            //    value.mensagem.Code = 5;
            //    value.mensagem.Message = MensagemPadrao.Message(5, "Valor").ToString();
            //    value.mensagem.MessageBase = "O valor não pode ser alterado nesta etapa";
            //    value.mensagem.MessageType = MsgType.WARNING;
            //    return value.mensagem;
            //}
            #endregion

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

            return (from p in db.Propostas 
                    join c in db.Clientes on p.clienteId equals c.clienteId
                    join emp in db.Empreendimentos on p.empreendimentoId equals emp.empreendimentoId
                    join est in db.Esteiras on p.propostaId equals est.propostaId 
                    join eta in db.Etapas on est.etapaId equals eta.etapaId
                    orderby p.dt_proposta, c.nome
                    select new PropostaViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        clienteId = p.clienteId,
                        cpf_cnpj = c.cpf_cnpj,
                        nome_cliente = c.nome,
                        propostaId = p.propostaId,
                        empreendimentoId = p.empreendimentoId,
                        descricao_empreendimento = emp.nomeEmpreend,
                        dt_proposta = p.dt_proposta,
                        unidade = p.unidade,
                        torre = p.torre,
                        valor = p.valor,
                        vr_comissao = p.vr_comissao,
                        etapaId = p.etapaId,
                        descricao_etapa = eta.descricao,
                        dt_ultimo_status = p.dt_ultimo_status,
                        operacaoId = p.operacaoId,
                        percent_atual = (eta.idx + 1.0) / (from et in db.Etapas select et.etapaId).Count() * 100,
                        percent_restnte = 100 - ((eta.idx + 1.0) / (from et in db.Etapas select et.etapaId).Count()) * 100,
                        PageSize = pageSize,
                        TotalCount = (from p1 in db.Propostas
                                      join c1 in db.Clientes on p1.clienteId equals c1.clienteId
                                      join emp1 in db.Empreendimentos on p1.empreendimentoId equals emp1.empreendimentoId
                                      join est1 in db.Esteiras on p1.propostaId equals est1.propostaId
                                      join eta1 in db.Etapas on est1.etapaId equals eta1.etapaId
                                      select p1.propostaId).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new PropostaModel().getObject((PropostaViewModel)id);
        }
        #endregion
    }

}