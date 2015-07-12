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

        #region Métodos da classe CrudContext
        public override Cliente MapToEntity(ClienteViewModel value)
        {
            return new Cliente()
            {
                clienteId = value.clienteId,
                nome = value.nome,
                ind_tipo_pessoa = value.ind_tipo_pessoa != null ? value.ind_tipo_pessoa.Substring(1, 1) : "F",
                cpf_cnpj = value.cpf_cnpj != null ? value.cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "") : null,
                dt_inclusao = value.dt_inclusao > Convert.ToDateTime("1980-01-01") ? value.dt_inclusao : Funcoes.Brasilia(),
                dt_alteracao = Funcoes.Brasilia(),
                endereco = value.endereco,
                complemento = value.complemento,
                cidade = value.cidade,
                uf = value.uf,
                cep = value.cep != null ? value.cep.Replace(".", "").Replace("-", "") : null,
                bairro = value.bairro,
                fone1 = value.fone1 != null ? value.fone1.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : null,
                fone2 = value.fone2 != null ? value.fone2.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : null,
                fone3 = value.fone3 != null ? value.fone3.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : null,
                email = value.email != null ? value.email.ToLower() : null,
                sexo = value.sexo,
                dt_nascimento = value.dt_nascimento,
                observacao = value.observacao
            };
        }

        public override ClienteViewModel MapToRepository(Cliente entity)
        {
            return new ClienteViewModel()
            {
                clienteId = entity.clienteId,
                nome = entity.nome,
                ind_tipo_pessoa = "P" + entity.ind_tipo_pessoa,
                cpf_cnpj = entity.cpf_cnpj,
                dt_inclusao = entity.dt_inclusao,
                dt_alteracao = entity.dt_alteracao,
                endereco = entity.endereco,
                complemento = entity.complemento,
                cidade = entity.cidade,
                uf = entity.uf,
                cep = entity.cep,
                bairro = entity.bairro,
                fone1 = entity.fone1,
                fone2 = entity.fone2,
                fone3 = entity.fone3,
                email = entity.email,
                sexo = entity.sexo,
                dt_nascimento = entity.dt_nascimento,
                observacao = entity.observacao,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Cliente Find(ClienteViewModel key)
        {
            return db.Clientes.Find(key.clienteId);
        }

        public override Validate Validate(ClienteViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString(), MessageType = MsgType.SUCCESS };

            if (value.nome.Trim().Length == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome").ToString();
                value.mensagem.MessageBase = "Campo Nome do Cliente deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #region Valida CPF/CNPJ
            if (value.cpf_cnpj != null)
            {
                // CPF
                if (value.cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Length == 11)
                {
                    if (!Funcoes.ValidaCpf(value.cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "")))
                    {
                        value.mensagem.Code = 29;
                        value.mensagem.Message = MensagemPadrao.Message(29).ToString();
                        value.mensagem.MessageBase = "Número de CPF incorreto.";
                        return value.mensagem;
                    }
                } // CNPJ
                else if (!Funcoes.ValidaCnpj(value.cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "")))
                {
                    value.mensagem.Code = 30;
                    value.mensagem.Message = MensagemPadrao.Message(30).ToString();
                    value.mensagem.MessageBase = "Número de CNPJ incorreto.";
                    return value.mensagem;
                }
                if (operation == Crud.ALTERAR)
                {
                    if (db.Clientes.Where(info => info.cpf_cnpj == value.cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "") && info.clienteId != value.clienteId).Count() > 0)
                    {
                        value.mensagem.Code = 31;
                        value.mensagem.Message = MensagemPadrao.Message(31).ToString();
                        value.mensagem.MessageBase = "CPF/CNPJ informado para o fornecedor já se encontra cadastrado para outro fornecedor.";
                        return value.mensagem;
                    }
                }
                else
                {
                    if (db.Clientes.Where(info => info.cpf_cnpj == value.cpf_cnpj.Replace(".", "").Replace("-", "").Replace("/", "")).Count() > 0)
                    {
                        value.mensagem.Code = 31;
                        value.mensagem.Message = MensagemPadrao.Message(31).ToString();
                        value.mensagem.MessageBase = "CPF/CNPJ informado para o fornecedor já se encontra cadastrado para outro fornecedor.";
                        return value.mensagem;
                    }
                }
            }
            #endregion

            return value.mensagem;
        }

        public override ClienteViewModel CreateRepository(System.Web.HttpRequestBase Request = null)
        {
            ClienteViewModel c = base.CreateRepository(Request);
            c.dt_inclusao = Funcoes.Brasilia();
            c.ind_tipo_pessoa = "PJ";
            return c;
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
            return (from clnt in db.Clientes
                    where (_nome == null || String.IsNullOrEmpty(_nome) || clnt.nome.Contains(_nome.Trim()) || clnt.cpf_cnpj == _nome)
                    orderby clnt.nome
                    select new ClienteViewModel
                    {
                        clienteId = clnt.clienteId,
                        cpf_cnpj = clnt.cpf_cnpj,
                        nome = clnt.nome,
                        fone1 = clnt.fone1,
                        fone2 = clnt.fone2,
                        email = clnt.email,
                        endereco = clnt.endereco,
                        PageSize = pageSize,
                        TotalCount = (from clnt1 in db.Clientes
                                      where (_nome == null || String.IsNullOrEmpty(_nome) || clnt1.nome.Contains(_nome.Trim()) || clnt1.cpf_cnpj == _nome)
                                      select clnt1.clienteId).Count()
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
            return "div-cli";
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