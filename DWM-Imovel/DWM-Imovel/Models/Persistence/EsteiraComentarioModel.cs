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
            Usuario u = seguranca_db.Usuarios.Find(sessaoCorrente.usuarioId);

            value.dt_comentario = Funcoes.Brasilia();
            value.usuarioId = sessaoCorrente.usuarioId;
            value.nome = u.nome;
            value.login = u.login;

            return value;
        }

        #region Métodos da classe CrudModel
        public override EsteiraComentario MapToEntity(EsteiraComentarioViewModel value)
        {
            EsteiraComentario comentario = new EsteiraComentario()
            {
                esteiraId = value.esteiraId,
                dt_comentario = value.dt_comentario,
                observacao = value.observacao,
                usuarioId = value.usuarioId,
                nome = value.nome,
                login = value.login
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

    public class ListViewComentario : ListViewModel<EsteiraComentarioViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewComentario() { }
        public ListViewComentario(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<EsteiraComentarioViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _esteiraId = int.Parse(param[0].ToString());

            return (from com in db.Comentarios
                    join est in db.Esteiras on com.esteiraId equals est.esteiraId
                    join eta in db.Etapas on est.etapaId equals eta.etapaId
                    where est.propostaId == (from e in db.Esteiras where e.esteiraId == _esteiraId select e.propostaId).FirstOrDefault()
                    orderby com.dt_comentario descending
                    select new EsteiraComentarioViewModel()
                    {
                        esteiraId = est.esteiraId,
                        descricao_etapa = eta.descricao,
                        dt_comentario = com.dt_comentario,
                        observacao = com.observacao,
                        usuarioId = com.usuarioId,
                        nome = com.nome,
                        login = com.login,
                        PageSize = pageSize,
                        TotalCount = (from com1 in db.Comentarios
                                      join est1 in db.Esteiras on com1.esteiraId equals est1.esteiraId
                                      join eta1 in db.Etapas on est1.etapaId equals eta1.etapaId
                                      where est1.propostaId == (from e1 in db.Esteiras where e1.esteiraId == _esteiraId select e1.propostaId).FirstOrDefault()
                                      orderby com1.dt_comentario descending
                                      select com1.esteiraId).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new EsteiraComentarioModel().getObject((EsteiraComentarioViewModel)id);
        }

        public override string action()
        {
            return "../Workflow/ListComentarios";
        }

        public override string DivId()
        {
            return "comentarios";
        }

        #endregion
    }

    public class ListViewComentarioByUsuario : ListViewModel<EsteiraComentarioViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewComentarioByUsuario() { }
        public ListViewComentarioByUsuario(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<EsteiraComentarioViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = null;
            if (param != null)
                if (param.Count() > 0)
                    if (param[0] != null)
                        _nome = param[0].ToString();

            return (from com in db.Comentarios
                    join est in db.Esteiras on com.esteiraId equals est.esteiraId
                    join eta in db.Etapas on est.etapaId equals eta.etapaId
                    join pro in db.Propostas on est.propostaId equals pro.propostaId
                    join cli in db.Clientes on pro.clienteId equals cli.clienteId
                    join emp in db.Empreendimentos on pro.empreendimentoId equals emp.empreendimentoId
                    where (_nome == null || _nome == "" || com.nome.Contains(_nome))
                            && pro.situacao == "A"
                    orderby com.dt_comentario descending
                    select new EsteiraComentarioViewModel()
                    {
                        esteiraId = est.esteiraId,
                        propostaId = pro.propostaId,
                        descricao_etapa = eta.descricao,
                        dt_comentario = com.dt_comentario,
                        observacao = com.observacao,
                        usuarioId = com.usuarioId,
                        nome = com.nome,
                        login = com.login,
                        nome_cliente = cli.nome,
                        nome_empreendimento = emp.nomeEmpreend,
                        PageSize = pageSize,
                        TotalCount = (from com1 in db.Comentarios
                                      join est1 in db.Esteiras on com1.esteiraId equals est1.esteiraId
                                      join eta1 in db.Etapas on est1.etapaId equals eta1.etapaId
                                      join pro1 in db.Propostas on est1.propostaId equals pro1.propostaId
                                      join cli1 in db.Clientes on pro1.clienteId equals cli1.clienteId
                                      join emp1 in db.Empreendimentos on pro1.empreendimentoId equals emp1.empreendimentoId
                                      where (_nome == null || _nome == "" || com1.nome.Contains(_nome))
                                            && pro1.situacao == "A"
                                      orderby com1.dt_comentario descending
                                      select com1.esteiraId).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new EsteiraComentarioModel().getObject((EsteiraComentarioViewModel)id);
        }

        public override string action()
        {
            return "../Home/ListAllComentarios";
        }

        public override string DivId()
        {
            return "div-comentarios";
        }

        #endregion
    }

}

