using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using App_Dominio.Enumeracoes;
using DWM.Models.Entidades;
using DWM.Models.Repositories;
using System.IO;
using App_Dominio.Security;
using App_Dominio.App_Start;
//using System.Data.Entity.SqlServer;

namespace DWM.Models.Persistence
{
    public class EsteiraComissaoModel : CrudModel<EsteiraComissao, EsteiraComissaoViewModel, ApplicationContext>
    {
        #region Constructor
        public EsteiraComissaoModel() { }
        public EsteiraComissaoModel(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe CrudModel

        public override EsteiraComissao MapToEntity(EsteiraComissaoViewModel value)
        {
            EsteiraComissao ec = Find(value);

            if (ec == null)
            {
                ec = new EsteiraComissao();
                ec.esteiraId = value.esteiraId;
                ec.grupoId = value.grupoId;
            }

            ec.nome_grupo= value.nome_grupo;
            ec.valor = value.valor;

            return ec;
        }

        public override EsteiraComissaoViewModel MapToRepository(EsteiraComissao entity)
        {
            ComissaoDefault cd = db.ComissaoDefaults.Find(entity.grupoId);

            int? _usuarioId = null;
            string _nome = "";
            string _login = "";

            switch (cd.source)
            {
                case 0: // Empreendimento
                    Empreendimento emp = (from est in db.Esteiras join pro in db.Propostas on est.propostaId equals pro.propostaId
                                          join empreend in db.Empreendimentos on pro.empreendimentoId equals empreend.empreendimentoId
                                          where est.esteiraId == entity.esteiraId
                                          select empreend).FirstOrDefault();
                    _usuarioId = emp.usuarioId;
                    _nome = emp.nome;
                    _login = emp.login;
                    break;
                case 1: // Proposta
                    Proposta prop = (from est in db.Esteiras join pro in db.Propostas on est.propostaId equals pro.propostaId
                                     where est.esteiraId == entity.esteiraId
                                     select pro).FirstOrDefault();
                    _usuarioId = prop.usuarioId;
                    _nome = prop.nome;
                    _login = prop.login;
                    break;
                case 2: // Corretor
                    Corretor corr = (from est in db.Esteiras join pro in db.Propostas on est.propostaId equals pro.propostaId
                                     join cor in db.Corretores on pro.corretor1Id equals cor.corretorId
                                     where est.esteiraId == entity.esteiraId
                                     select cor).FirstOrDefault();
                    _nome = corr.nome;
                    _login = corr.email;
                    break;
                case 3: // Imobiliária
                    Empresa e = seguranca_db.Empresas.Find(sessaoCorrente.empresaId);
                    _nome = e.nome;
                    _login = e.email;

                    break;
            }

            return new EsteiraComissaoViewModel()
            {
                esteiraId = entity.esteiraId,
                grupoId = entity.grupoId,
                nome_grupo = entity.nome_grupo,
                valor = entity.valor,
                usuarioId = _usuarioId,
                nome = _nome,
                login = _login,
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };
        }

        public override EsteiraComissao Find(EsteiraComissaoViewModel key)
        {
            return db.EsteiraComissaos.Find(key.esteiraId, key.grupoId);
        }

        public override Validate Validate(EsteiraComissaoViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if ((operation == Crud.ALTERAR || operation == Crud.EXCLUIR) && value.esteiraId == 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Esteira ID").ToString();
                value.mensagem.MessageBase = "Identificador da esteira deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if ((operation == Crud.ALTERAR || operation == Crud.EXCLUIR) && (value.grupoId == null || value.grupoId == 0))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Arquivo").ToString();
                value.mensagem.MessageBase = "Grupo (Gerente, corretor, coordenador) deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.nome_grupo == null || value.nome_grupo == "")
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Nome do arquivo").ToString();
                value.mensagem.MessageBase = "Nome do grupo deve ser informado";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.valor < 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Valor da comissão").ToString();
                value.mensagem.MessageBase = "Valor da comissão deve ser informado e deve ser um valor maior ou igual a zero";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            return value.mensagem;
        }
        #endregion

    }

    public class ListViewEsteiraComissao : ListViewModel<EsteiraComissaoViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewEsteiraComissao() { }
        public ListViewEsteiraComissao(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<EsteiraComissaoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _esteiraId = int.Parse(param[0].ToString());

            #region verifica se a etapa atual é menor que a etapa de comissão. Se for, não deve retornar nenhuma lista
            // recupera a proposta
            int _propostaId = (from es in db.Esteiras where es.esteiraId == _esteiraId select es.propostaId).FirstOrDefault();
            Proposta proposta = db.Propostas.Find(_propostaId);

            #region Verifica se tem Etapa específica para o empreendimento. Se não tiver, trás a etapa "Proposta" padrão para todos os empreendimentos
            int _etapaId;
            string _descricao = DWM.Models.Enumeracoes.Enumeradores.DescricaoEtapa.COMISSAO.GetStringValue();
            if (db.Etapas.Where(info => info.empreendimentoId == proposta.empreendimentoId && info.descricao == _descricao).Count() > 0)
                _etapaId = db.Etapas.Where(info => info.empreendimentoId == proposta.empreendimentoId && info.descricao == _descricao).FirstOrDefault().etapaId;
            else
                _etapaId = db.Etapas.Where(info => info.descricao == _descricao).FirstOrDefault().etapaId;
            #endregion

            if (proposta.etapaId < _etapaId)
                return new List<EsteiraComissaoViewModel>();
            #endregion


            #region Usuario comissao
            int?[] _usuarioId = new int?[5] { null, null, null, null, null};
            string[] _nome = new string[5] { "", "", "", "", "" };
            string[] _login = new string[5] { "", "", "", "", "" };

            //0-Empreendimento
            Empreendimento emp = (from est in db.Esteiras
                                  join pro in db.Propostas on est.propostaId equals pro.propostaId
                                  join empreend in db.Empreendimentos on pro.empreendimentoId equals empreend.empreendimentoId
                                  where est.esteiraId == _esteiraId
                                  select empreend).FirstOrDefault();
            _usuarioId [0] = emp.usuarioId;
            _nome [0] = emp.nome;
            _login [0] = emp.login ?? "";

            // 1-Proposta
            Proposta prop = (from est in db.Esteiras
                             join pro in db.Propostas on est.propostaId equals pro.propostaId
                             where est.esteiraId == _esteiraId
                             select pro).FirstOrDefault();
            _usuarioId[1] = prop.usuarioId;
            _nome[1] = prop.nome;
            _login[1] = prop.login ?? "";

            // 2-Corretor
            Corretor corr = (from est in db.Esteiras
                             join pro in db.Propostas on est.propostaId equals pro.propostaId
                             join cor in db.Corretores on pro.corretor1Id equals cor.corretorId
                             where est.esteiraId == _esteiraId
                             select cor).FirstOrDefault();
            _nome[2] = corr.nome;
            _login[2] = corr.email ?? "";

            // 3-Imobiliária
            Empresa e = seguranca_db.Empresas.Find(sessaoCorrente.empresaId);
            _nome[3] = e.nome;
            _login[3] = e.email ?? "";
            #endregion

            IEnumerable<EsteiraComissaoViewModel> list = (from com in db.EsteiraComissaos
                                                          join est in db.Esteiras on com.esteiraId equals est.esteiraId
                                                          join comdef in db.ComissaoDefaults on com.grupoId equals comdef.grupoId
                                                          where est.esteiraId == _esteiraId
                                                                && est.propostaId == (from es in db.Esteiras where es.esteiraId == _esteiraId select es.propostaId).FirstOrDefault()
                                                          orderby com.valor
                                                          select new EsteiraComissaoViewModel()
                                                          {
                                                              esteiraId = est.esteiraId,
                                                              grupoId = com.grupoId,
                                                              nome_grupo = com.nome_grupo,
                                                              valor = com.valor,
                                                              PageSize = pageSize,
                                                              TotalCount = (from com1 in db.EsteiraComissaos
                                                                            join est1 in db.Esteiras on com1.esteiraId equals est1.esteiraId
                                                                            join comdef1 in db.ComissaoDefaults on com1.grupoId equals comdef1.grupoId
                                                                            where est1.esteiraId == _esteiraId
                                                                                    && est1.propostaId == (from es1 in db.Esteiras where es1.esteiraId == _esteiraId select es1.propostaId).FirstOrDefault()
                                                                            orderby com1.valor
                                                                            select est1.esteiraId).Count()
                                                          }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();

            for (int i = 0; i <= list.Count() - 1; i++ )
            {
                int x = db.ComissaoDefaults.Find(list.ElementAt(i).grupoId).source;
                list.ElementAt(i).usuarioId = (from _u in _usuarioId select _u).ElementAt(x).HasValue ? (from _u in _usuarioId select _u).ElementAt(x).Value : 0;
                list.ElementAt(i).nome = (from _n in _nome select _n).ElementAt(x) ?? "";
                list.ElementAt(i).login = (from _l in _login select _l).ElementAt(x) ?? "";
            };

            return list;
        }

        public override Repository getRepository(Object id)
        {
            return new EsteiraComissaoModel().getObject((EsteiraComissaoViewModel)id);
        }

        public override string action()
        {
            return "../Workflow/ListComissao";
        }

        public override string DivId()
        {
            return "comissao";
        }

        #endregion
    }

    public class ListViewComissao : ListViewModel<EsteiraComissaoViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewComissao() { }
        public ListViewComissao(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<EsteiraComissaoViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _esteiraId = int.Parse(param[0].ToString());

            #region verifica se a etapa atual é menor que a etapa de comissão. Se for, não deve retornar nenhuma lista
            // recupera a proposta
            int _propostaId = (from es in db.Esteiras where es.esteiraId == _esteiraId select es.propostaId).FirstOrDefault();
            Proposta proposta = db.Propostas.Find(_propostaId);

            #region Verifica se tem Etapa específica para o empreendimento. Se não tiver, trás a etapa "Proposta" padrão para todos os empreendimentos
            int _etapaId;
            string _descricao = DWM.Models.Enumeracoes.Enumeradores.DescricaoEtapa.COMISSAO.GetStringValue();
            if (db.Etapas.Where(info => info.empreendimentoId == proposta.empreendimentoId && info.descricao == _descricao).Count() > 0)
                _etapaId = db.Etapas.Where(info => info.empreendimentoId == proposta.empreendimentoId && info.descricao == _descricao).FirstOrDefault().etapaId;
            else
                _etapaId = db.Etapas.Where(info => info.descricao == _descricao).FirstOrDefault().etapaId;
            #endregion

            if (proposta.etapaId < _etapaId)
                return new List<EsteiraComissaoViewModel>();
            #endregion

            #region Usuario comissao
            int?[] _usuarioId = new int?[5] { null, null, null, null, null };
            string[] _nome = new string[5] { "", "", "", "", "" };
            string[] _login = new string[5] { "", "", "", "", "" };

            //0-Empreendimento
            Empreendimento emp = (from est in db.Esteiras
                                  join pro in db.Propostas on est.propostaId equals pro.propostaId
                                  join empreend in db.Empreendimentos on pro.empreendimentoId equals empreend.empreendimentoId
                                  where est.esteiraId == _esteiraId
                                  select empreend).FirstOrDefault();
            _usuarioId[0] = emp.usuarioId;
            _nome[0] = emp.nome;
            _login[0] = emp.login ?? "";

            // 1-Proposta
            Proposta prop = (from est in db.Esteiras
                             join pro in db.Propostas on est.propostaId equals pro.propostaId
                             where est.esteiraId == _esteiraId
                             select pro).FirstOrDefault();
            _usuarioId[1] = prop.usuarioId;
            _nome[1] = prop.nome;
            _login[1] = prop.login ?? "";

            // 2-Corretor
            Corretor corr = (from est in db.Esteiras
                             join pro in db.Propostas on est.propostaId equals pro.propostaId
                             join cor in db.Corretores on pro.corretor1Id equals cor.corretorId
                             where est.esteiraId == _esteiraId
                             select cor).FirstOrDefault();
            _nome[2] = corr.nome;
            _login[2] = corr.email ?? "";

            // 3-Imobiliária
            Empresa e = seguranca_db.Empresas.Find(sessaoCorrente.empresaId);
            _nome[3] = e.nome;
            _login[3] = e.email ?? "";
            #endregion

            IEnumerable<EsteiraComissaoViewModel> list = (from com in db.EsteiraComissaos
                                                          join est in db.Esteiras on com.esteiraId equals est.esteiraId
                                                          join comdef in db.ComissaoDefaults on com.grupoId equals comdef.grupoId
                                                          where est.ind_aprovacao != "R"
                                                                && est.propostaId == (from es in db.Esteiras where es.esteiraId == _esteiraId select es.propostaId).FirstOrDefault()
                                                                && com.esteiraId == (from comMax in db.EsteiraComissaos
                                                                                     join estMax in db.Esteiras on comMax.esteiraId equals estMax.esteiraId
                                                                                     where estMax.propostaId == (from esMax in db.Esteiras where esMax.esteiraId == _esteiraId select esMax.propostaId).FirstOrDefault()
                                                                                     select comMax.esteiraId).Max()
                                                          orderby com.valor
                                                          select new EsteiraComissaoViewModel()
                                                          {
                                                              esteiraId = est.esteiraId,
                                                              grupoId = com.grupoId,
                                                              nome_grupo = com.nome_grupo,
                                                              valor = com.valor,
                                                              PageSize = pageSize,
                                                              TotalCount = (from com1 in db.EsteiraComissaos
                                                                            join est1 in db.Esteiras on com1.esteiraId equals est1.esteiraId
                                                                            join comdef1 in db.ComissaoDefaults on com1.grupoId equals comdef1.grupoId
                                                                            where est1.ind_aprovacao != "R"
                                                                                  && est1.propostaId == (from es1 in db.Esteiras where es1.esteiraId == _esteiraId select es1.propostaId).FirstOrDefault()
                                                                                  && com1.esteiraId ==  (from comMax1 in db.EsteiraComissaos
                                                                                                         join estMax1 in db.Esteiras on comMax1.esteiraId equals estMax1.esteiraId
                                                                                                         where estMax1.propostaId == (from esMax1 in db.Esteiras where esMax1.esteiraId == _esteiraId select esMax1.propostaId).FirstOrDefault()
                                                                                                         select comMax1.esteiraId).Max()

                                                                            orderby com1.valor
                                                                            select est1.esteiraId).Count()
                                                          }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();

            for (int i = 0; i <= list.Count() - 1; i++)
            {
                int x = db.ComissaoDefaults.Find(list.ElementAt(i).grupoId).source;
                list.ElementAt(i).usuarioId = (from _u in _usuarioId select _u).ElementAt(x).HasValue ? (from _u in _usuarioId select _u).ElementAt(x).Value : 0;
                list.ElementAt(i).nome = (from _n in _nome select _n).ElementAt(x) ?? "";
                list.ElementAt(i).login = (from _l in _login select _l).ElementAt(x) ?? "";
            };

            return list;
        }

        public override Repository getRepository(Object id)
        {
            return new EsteiraComissaoModel().getObject((EsteiraComissaoViewModel)id);
        }

        public override string action()
        {
            return "../Workflow/ListComissao";
        }

        public override string DivId()
        {
            return "comissao";
        }

        #endregion
    }
}