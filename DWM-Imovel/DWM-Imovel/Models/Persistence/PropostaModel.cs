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
        public override Proposta MapToEntity(PropostaViewModel value)
        {
            return new Proposta()
            {
                propostaId = value.propostaId,
                empreendimentoId = value.empreendimentoId,
                clienteId = value.clienteId,
                dt_proposta = value.dt_proposta,
                unidade = value.unidade,
                modelo = value.unidade,
                valor = value.valor,
                vr_comissao = value.vr_comissao,
                etapaId = value.etapaId,
                dt_ultimo_status = value.dt_ultimo_status,
                operacaoId = value.operacaoId
            };
        }

        public override PropostaViewModel MapToRepository(Proposta value)
        {
            return new PropostaViewModel()
            {
                propostaId = value.propostaId,
                empreendimentoId = value.empreendimentoId,
                descricao_empreendimento = value.empreendimentoId != null ? db.Empreendimentos.Find(value.empreendimentoId).nome : "",
                clienteId = value.clienteId,
                descricao_cliente = value.clienteId != null ? db.Clientes.Find(value.clienteId).nome : "",
                dt_proposta = value.dt_proposta,
                unidade = value.unidade,
                modelo = value.unidade,
                valor = value.valor,
                vr_comissao = value.vr_comissao,
                etapaId = value.etapaId,
                dt_ultimo_status = value.dt_ultimo_status,
                operacaoId = value.operacaoId,
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
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Cliente").ToString();
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
                    where (_nome == null || String.IsNullOrEmpty(_nome) || c.modelo.StartsWith(_nome.Trim()))
                    orderby c.modelo
                    select new PropostaViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        clienteId = c.clienteId,
                        propostaId = c.propostaId,
                        empreendimentoId = c.empreendimentoId,
                        dt_proposta = c.dt_proposta,
                        unidade = c.unidade,
                        modelo = c.unidade,
                        valor = c.valor,
                        vr_comissao = c.vr_comissao,
                        etapaId = c.etapaId,
                        dt_ultimo_status = c.dt_ultimo_status,
                        operacaoId = c.operacaoId,
                        PageSize = pageSize,
                        TotalCount = (from c1 in db.Propostas
                                      where (_nome == null || String.IsNullOrEmpty(_nome) || c1.modelo.StartsWith(_nome.Trim()))
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