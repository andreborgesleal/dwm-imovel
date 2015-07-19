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
    public class EsteiraModel : CrudModel<Esteira, EsteiraViewModel, ApplicationContext>
    {
        #region Constructor
        public EsteiraModel() { }
        public EsteiraModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override Esteira MapToEntity(EsteiraViewModel value)
        {
            return new Esteira()
            {
                esteiraId = value.esteiraId,
                propostaId = value.propostaId,
                dt_evento = value.dt_evento,
                etapaId = value.etapaId,
                dt_ocorrencia = value.dt_ocorrencia,
                ind_aprovacao = value.ind_aprovacao,
                observacao = value.observacao,
                usuarioId = value.usuarioId,
                nome = value.nome,
                login = value.login,
            };
        }

        public override EsteiraViewModel MapToRepository(Esteira entity)
        {
            return new EsteiraViewModel()
            {
                esteiraId = entity.esteiraId,
                propostaId = entity.propostaId,
                dt_evento = entity.dt_evento,
                etapaId = entity.etapaId,
                dt_ocorrencia = entity.dt_ocorrencia,
                ind_aprovacao = entity.ind_aprovacao,
                observacao = entity.observacao,
                usuarioId = entity.usuarioId,
                nome = entity.nome,
                login = entity.login,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Esteira Find(EsteiraViewModel key)
        {
            return db.Esteiras.Find(key.esteiraId);
        }

        public override Validate Validate(EsteiraViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.dt_evento == null)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Data do evento").ToString();
                value.mensagem.MessageBase = "Data do evento deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.dt_ocorrencia == null)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Data da Ocorrência").ToString();
                value.mensagem.MessageBase = "Data da ocorrência deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            return value.mensagem;
        }

        #endregion

    }
}