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
            #region verifica o perfil do usuário logado
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            security.seguranca_db = this.seguranca_db;
            string descricao_grupo = security._getUsuarioGrupo(sessaoCorrente.usuarioId).FirstOrDefault().descricao;
            #endregion

            string _nome = param != null && param.Count() > 0 && param[0] != null ? param[0].ToString() : null;
            int? _clienteId = null;

            if (param != null)
                if (param.Count() == 2)
                    _clienteId = param[1] != null ? (int?)param[1] : null;

            return (from q in
                       (from clnt in db.Clientes
                        join pro in db.Propostas on clnt.clienteId equals pro.clienteId
                        join emp in db.Empreendimentos on pro.empreendimentoId equals emp.empreendimentoId
                        join cor in db.Corretores on pro.corretor1Id equals cor.corretorId into COR
                        from cor in COR.DefaultIfEmpty()
                        where (_nome == null || String.IsNullOrEmpty(_nome) || clnt.nome.Contains(_nome.Trim()) || clnt.cpf_cnpj == _nome)
                              && (!_clienteId.HasValue || clnt.clienteId == _clienteId)
                              && ((descricao_grupo == "Corretor" && cor.email == sessaoCorrente.login) ||
                                  (descricao_grupo == "Coordenador" && emp.login == sessaoCorrente.login) ||
                                  (descricao_grupo == "Gerente de Equipe" && pro.login == sessaoCorrente.login) ||
                                  (!"Corretor|Coordenador|Gerente de Equipe".Contains(descricao_grupo)))
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
                            TotalCount = 0
                        }).Union(from cli in db.Clientes 
                                 join p in db.Propostas on cli.clienteId equals p.clienteId into P
                                 from p in P.DefaultIfEmpty()
                                 where (_nome == null || String.IsNullOrEmpty(_nome) || cli.nome.Contains(_nome.Trim()) || cli.cpf_cnpj == _nome)
                                       && (!_clienteId.HasValue || cli.clienteId == _clienteId)
                                       && p == null
                                 select new ClienteViewModel
                                 {
                                     clienteId = cli.clienteId,
                                     cpf_cnpj = cli.cpf_cnpj,
                                     nome = cli.nome,
                                     fone1 = cli.fone1,
                                     fone2 = cli.fone2,
                                     email = cli.email,
                                     endereco = cli.endereco,
                                     PageSize = pageSize,
                                     TotalCount = 0
                                 })
                    orderby q.nome
                    select new ClienteViewModel
                    {
                        clienteId = q.clienteId,
                        cpf_cnpj = q.cpf_cnpj,
                        nome = q.nome,
                        fone1 = q.fone1,
                        fone2 = q.fone2,
                        email = q.email,
                        endereco = q.endereco,
                        PageSize = pageSize,
                        TotalCount = (from q1 in (from clnt1 in db.Clientes
                                                  join pro1 in db.Propostas on clnt1.clienteId equals pro1.clienteId
                                                  join emp1 in db.Empreendimentos on pro1.empreendimentoId equals emp1.empreendimentoId
                                                  join cor1 in db.Corretores on pro1.corretor1Id equals cor1.corretorId into COR1
                                                  from cor1 in COR1.DefaultIfEmpty()
                                                  where (_nome == null || String.IsNullOrEmpty(_nome) || clnt1.nome.Contains(_nome.Trim()) || clnt1.cpf_cnpj == _nome)
                                                        && (!_clienteId.HasValue || clnt1.clienteId == _clienteId)
                                                        && ((descricao_grupo == "Corretor" && cor1.email == sessaoCorrente.login) ||
                                                            (descricao_grupo == "Coordenador" && emp1.login == sessaoCorrente.login) ||
                                                            (descricao_grupo == "Gerente de Equipe" && pro1.login == sessaoCorrente.login) ||
                                                            (!"Corretor|Coordenador|Gerente de Equipe".Contains(descricao_grupo)))
                                                  select new ClienteViewModel
                                                  {
                                                      clienteId = clnt1.clienteId,
                                                      cpf_cnpj = clnt1.cpf_cnpj,
                                                      nome = clnt1.nome,
                                                      fone1 = clnt1.fone1,
                                                      fone2 = clnt1.fone2,
                                                      email = clnt1.email,
                                                      endereco = clnt1.endereco,
                                                      PageSize = pageSize,
                                                      TotalCount = 0
                                                  }).Union(from cli1 in db.Clientes
                                                           join p1 in db.Propostas on cli1.clienteId equals p1.clienteId into P1
                                                           from p1 in P1.DefaultIfEmpty()
                                                           where (_nome == null || String.IsNullOrEmpty(_nome) || cli1.nome.Contains(_nome.Trim()) || cli1.cpf_cnpj == _nome)
                                                                 && (!_clienteId.HasValue || cli1.clienteId == _clienteId)
                                                                 && p1 == null
                                                           select new ClienteViewModel()
                                                           {
                                                               clienteId = cli1.clienteId,
                                                               cpf_cnpj = cli1.cpf_cnpj,
                                                               nome = cli1.nome,
                                                               fone1 = cli1.fone1,
                                                               fone2 = cli1.fone2,
                                                               email = cli1.email,
                                                               endereco = cli1.endereco,
                                                               PageSize = pageSize,
                                                               TotalCount = 0
                                                           })
                                      select q1).AsEnumerable().Count()
                    }
            ).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
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