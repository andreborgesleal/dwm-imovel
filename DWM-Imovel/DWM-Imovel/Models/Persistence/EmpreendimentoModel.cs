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
using DWM.Models.Entidades;
using App_Dominio.Security;

namespace DWM.Models.Persistence
{
    public class EmpreendimentoModel : CrudModel<Empreendimento, EmpreendimentoViewModel, ApplicationContext>
    {
        #region Constructor
        public EmpreendimentoModel() { }
        public EmpreendimentoModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override Empreendimento MapToEntity(EmpreendimentoViewModel value)
        {
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            Usuario u = security._getUsuarioById(value.usuarioId, seguranca_db);

            return new Empreendimento()
            {
                empreendimentoId = value.empreendimentoId,
                nomeEmpreend = value.nomeEmpreend,
                usuarioId = value.usuarioId,
                nome = u.nome,
                login = u.login
            };
        }

        public override EmpreendimentoViewModel MapToRepository(Empreendimento entity)
        {
            return new EmpreendimentoViewModel()
            {
                empreendimentoId = entity.empreendimentoId,
                nomeEmpreend = entity.nomeEmpreend,
                usuarioId = entity.usuarioId,
                nome = entity.nome,
                login = entity.login,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Empreendimento Find(EmpreendimentoViewModel key)
        {
            return db.Empreendimentos.Find(key.empreendimentoId);
        }

        public override Validate Validate(EmpreendimentoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.nomeEmpreend.Trim().Length == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Empreendimento").ToString();
                value.mensagem.MessageBase = "Nome do Empreendimento deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.INCLUIR || operation == Crud.ALTERAR)
            {
                int nomeEmpreendimento = (from c in db.Empreendimentos
                                  where c.empreendimentoId != value.empreendimentoId
                                        && c.nome.Equals(value.nome)
                                  select c.nome).Count();
                if (nomeEmpreendimento > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "Descrição já existente";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        #endregion
    }

    public class ListViewEmpreendimento : ListViewModel<EmpreendimentoViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewEmpreendimento() { }
        public ListViewEmpreendimento(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<EmpreendimentoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param.Count() > 0 && param[0] != null ? param[0].ToString() : null;
            return (from c in db.Empreendimentos
                    where (_nome == null || String.IsNullOrEmpty(_nome) || c.nome.StartsWith(_nome.Trim()))
                    orderby c.nome
                    select new EmpreendimentoViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        empreendimentoId = c.empreendimentoId,
                        usuarioId = c.usuarioId,
                        nomeEmpreend = c.nomeEmpreend,
                        login = c.login,
                        nome = c.nome,
                        PageSize = pageSize,
                        TotalCount = (from c1 in db.Empreendimentos
                                      where (_nome == null || String.IsNullOrEmpty(_nome) || c1.nome.StartsWith(_nome.Trim()))
                                      select c1).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new EmpreendimentoModel().getObject((EmpreendimentoViewModel)id);
        }
        #endregion
    }
}