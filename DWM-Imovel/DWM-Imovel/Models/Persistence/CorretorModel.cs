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
    public class CorretorModel : CrudModel<Corretor, CorretorViewModel, ApplicationContext>
    {
        #region Constructor
        public CorretorModel() { }
        public CorretorModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudContext
        public override Corretor MapToEntity(CorretorViewModel value)
        {
            return new Corretor()
            {
                corretorId = value.corretorId,
                nome = value.nome,
                cpf = value.cpf != null ? value.cpf.Replace(".", "").Replace("-", "").Replace("/", "") : null,
                rg = value.rg,
                orgao_emissor = value.orgao_emissor,
                creci = value.creci,
                endereco = value.endereco,
                complemento_end = value.complemento_end,
                cidade = value.cidade,
                uf = value.uf,
                cep = value.cep != null ? value.cep.Replace(".", "").Replace("-", "") : null,
                bairro = value.bairro,
                fone1 = value.fone1 != null ? value.fone1.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : null,
                fone2 = value.fone2 != null ? value.fone2.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : null,
                email = value.email != null ? value.email.ToLower() : null,
            };
        }

        public override CorretorViewModel MapToRepository(Corretor entity)
        {
            return new CorretorViewModel()
            {
                corretorId = entity.corretorId,
                nome = entity.nome,
                cpf = entity.cpf != null ? entity.cpf.Replace(".", "").Replace("-", "").Replace("/", "") : null,
                rg = entity.rg,
                orgao_emissor = entity.orgao_emissor,
                creci = entity.creci,
                endereco = entity.endereco,
                complemento_end = entity.complemento_end,
                cidade = entity.cidade,
                uf = entity.uf,
                cep = entity.cep != null ? entity.cep.Replace(".", "").Replace("-", "") : null,
                bairro = entity.bairro,
                fone1 = entity.fone1 != null ? entity.fone1.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : null,
                fone2 = entity.fone2 != null ? entity.fone2.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : null,
                email = entity.email != null ? entity.email.ToLower() : null,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override Corretor Find(CorretorViewModel key)
        {
            return db.Corretores.Find(key.corretorId);
        }

        public override Validate Validate(CorretorViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString(), MessageType = MsgType.SUCCESS };

            if (value.nome.Trim().Length == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome").ToString();
                value.mensagem.MessageBase = "Campo Nome do Corretor deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #region Valida CPF/CNPJ
            if (value.cpf != null)
            {
                // CPF
                if (value.cpf.Replace(".", "").Replace("-", "").Replace("/", "").Length == 11)
                {
                    if (!Funcoes.ValidaCpf(value.cpf.Replace(".", "").Replace("-", "").Replace("/", "")))
                    {
                        value.mensagem.Code = 29;
                        value.mensagem.Message = MensagemPadrao.Message(29).ToString();
                        value.mensagem.MessageBase = "Número de CPF incorreto.";
                        return value.mensagem;
                    }
                } 

                if (operation == Crud.ALTERAR)
                {
                    if (db.Corretores.Where(info => info.cpf == value.cpf.Replace(".", "").Replace("-", "").Replace("/", "") && info.corretorId != value.corretorId).Count() > 0)
                    {
                        value.mensagem.Code = 31;
                        value.mensagem.Message = MensagemPadrao.Message(31).ToString();
                        value.mensagem.MessageBase = "CPF informado para o corretor já se encontra cadastrado para outro corretor.";
                        return value.mensagem;
                    }
                }
                else
                {
                    if (db.Corretores.Where(info => info.cpf == value.cpf.Replace(".", "").Replace("-", "").Replace("/", "")).Count() > 0)
                    {
                        value.mensagem.Code = 31;
                        value.mensagem.Message = MensagemPadrao.Message(31).ToString();
                        value.mensagem.MessageBase = "CPF informado para o corretor já se encontra cadastrado para outro corretor.";
                        return value.mensagem;
                    }
                }
            }
            #endregion

            return value.mensagem;
        }

        #endregion
    }

    public class ListViewCorretor : ListViewModel<CorretorViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewCorretor () { }
        public ListViewCorretor(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<CorretorViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            string _nome = param != null && param.Count() > 0 && param[0] != null ? param[0].ToString() : null;
            return (from clnt in db.Corretores
                    where (_nome == null || String.IsNullOrEmpty(_nome) || clnt.nome.Contains(_nome.Trim()) || clnt.cpf == _nome)
                    orderby clnt.nome
                    select new CorretorViewModel
                    {
                        corretorId = clnt.corretorId,
                        nome = clnt.nome,
                        cpf = clnt.cpf,
                        rg = clnt.rg,
                        orgao_emissor = clnt.orgao_emissor,
                        creci = clnt.creci,
                        endereco = clnt.endereco,
                        complemento_end = clnt.complemento_end,
                        cidade = clnt.cidade,
                        uf = clnt.uf,
                        cep = clnt.cep,
                        bairro = clnt.bairro,
                        fone1 = clnt.fone1,
                        fone2 = clnt.fone2,
                        email = clnt.email,
                        PageSize = pageSize,
                        TotalCount = (from clnt1 in db.Corretores
                                      where (_nome == null || String.IsNullOrEmpty(_nome) || clnt1.nome.Contains(_nome.Trim()) || clnt1.cpf == _nome)
                                      select clnt1.corretorId).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new CorretorModel().getObject((CorretorViewModel)id);
        }
        #endregion
    }

    public class LookupCorretorModel : ListViewCorretor
    {
        public override string action()
        {
            return "../Corretores/ListCorretorModal";
        }

        public override string DivId()
        {
            return "div-cor";
        }
    }

    public class LookupCorretorFiltroModel : ListViewCorretor
    {
        public override string action()
        {
            return "../Corretores/_ListCorretorModal";
        }

        public override string DivId()
        {
            return "div-cor";
        }
    }

}