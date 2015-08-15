using App_Dominio.App_Start;
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
    public class EsteiraComentarioModel: CrudModel<EsteiraComentario, EsteiraComentarioViewModel, ApplicationContext>
    {
        #region Constructor
        public EsteiraComentarioModel() { }
        public EsteiraComentarioModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        public override EsteiraComentarioViewModel BeforeInsert(EsteiraComentarioViewModel value)
        {
            value.dt_comentario = Funcoes.Brasilia();

            return value;
        }

        #region Métodos da classe CrudModel
        public override EsteiraComentario MapToEntity(EsteiraComentarioViewModel value)
        {
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            Usuario u = security._getUsuarioById(value.usuarioId, seguranca_db);

            EsteiraComentario comentario = new EsteiraComentario()
            {
                esteiraId = value.esteiraId,
                dt_comentario = value.dt_comentario,
                observacao = value.observacao,
                usuarioId = value.usuarioId,
                nome = u.nome,
                login = u.login
            };

            return comentario;
        }

        public override EsteiraComentarioViewModel MapToRepository(EsteiraComentario entity)
        {
            return new EsteiraComentarioViewModel()
            {
                esteiraId = entity.esteiraId,
                dt_comentario = entity.dt_comentario,
                observacao = entity.observacao,
                usuarioId = entity.usuarioId,
                nome = entity.nome,
                login = entity.login,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override EsteiraComentario Find(EsteiraComentarioViewModel key)
        {
            return db.Comentarios.Find(new { key.esteiraId, key.dt_comentario});
        }

        public override Validate Validate(EsteiraComentarioViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation == Crud.ALTERAR || operation == Crud.EXCLUIR && value.esteiraId == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Esteira ID").ToString();
                value.mensagem.MessageBase = "Identificador da esteira deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.ALTERAR || operation == Crud.EXCLUIR && value.dt_comentario <= Convert.ToDateTime("1980-01-01"))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Esteira ID").ToString();
                value.mensagem.MessageBase = "Data do comentário deve ser informada";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.observacao.Trim() == "")
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Comentário").ToString();
                value.mensagem.MessageBase = "Comentário deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.usuarioId == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Usuário").ToString();
                value.mensagem.MessageBase = "Identificador do usuário dever ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.nome.Trim() == "")
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do usuário").ToString();
                value.mensagem.MessageBase = "Nome do usuário dever ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.login.Trim() == "")
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Login").ToString();
                value.mensagem.MessageBase = "Login do usuário dever ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            return value.mensagem;
        }
        #endregion
    }
}

