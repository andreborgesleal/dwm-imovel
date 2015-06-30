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
    public class ClienteModel : CrudModel<Cliente, ClienteViewModel, ApplicationContext>
    {

        #region Constructor
        public ClienteModel() { }
        public ClienteModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel
        public override Cliente MapToEntity(ClienteViewModel value)
        {
            return new Cliente()
            {
                clienteId = value.clienteId,
                nome = value.nome,
                cpf_cnpj = value.cpf_cnpj,
                telefone = value.telefone != null ? value.telefone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : null
            };
        }

        public override ClienteViewModel MapToRepository(Cliente entity)
        {
            return new ClienteViewModel()
            {
                clienteId = entity.clienteId,
                nome = entity.nome,
                cpf_cnpj = entity.cpf_cnpj,
                telefone = entity.telefone,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Cliente Find(ClienteViewModel key)
        {
            return db.Clientes.Find(key.clienteId);
        }

        public override Validate Validate(ClienteViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (value.nome.Trim().Length == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do Cliente").ToString();
                value.mensagem.MessageBase = "Nome do Cliente deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.cpf_cnpj.Trim().Length == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "CPF/CNPJ").ToString();
                value.mensagem.MessageBase = "CPF/CPNJ do Cliente deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (operation == Crud.INCLUIR || operation == Crud.ALTERAR)
            {
                int nomeCliente = (from c in db.Clientes
                                          where c.clienteId != value.clienteId
                                                && c.nome.Equals(value.nome)
                                          select c.nome).Count();
                if (nomeCliente > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "nome do cliente já existente";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            return value.mensagem;
        }

        #endregion
    }

    public class ListViewCliente : ListViewModel<ClienteViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewCliente() { }
        public ListViewCliente(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<ClienteViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param.Count() > 0 && param[0] != null ? param[0].ToString() : null;
            return (from c in db.Clientes
                    where (_nome == null || String.IsNullOrEmpty(_nome) || c.nome.StartsWith(_nome.Trim()))
                    orderby c.nome
                    select new ClienteViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        clienteId = c.clienteId,
                        cpf_cnpj = c.cpf_cnpj,
                        telefone = c.telefone,
                        nome = c.nome,
                        PageSize = pageSize,
                        TotalCount = (from c1 in db.Clientes
                                      where (_nome == null || String.IsNullOrEmpty(_nome) || c1.nome.StartsWith(_nome.Trim()))
                                      select c1).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new ClienteModel().getObject((ClienteViewModel)id);
        }
        #endregion
    }


    public class LookupClienteModel : ListViewCliente
    {
        public override string action()
        {
            return "../Clientes/ListClienteModal";
        }

        public override string DivId()
        {
            return "div-ccu";
        }
    }

    public class LookupClienteFiltroModel : ListViewCliente
    {
        public override string action()
        {
            return "../Clientes/_ListClienteModal";
        }

        public override string DivId()
        {
            return "div-ccu";
        }
    }

}