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
using App_Dominio.Repositories;

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
        public override PropostaViewModel BeforeInsert(PropostaViewModel value)
        {
            Usuario u = seguranca_db.Usuarios.Find(sessaoCorrente.usuarioId);

            value.usuarioId = sessaoCorrente.usuarioId;
            value.nome = u.nome;
            value.login = u.login;
            value.dt_ultimo_status = value.dt_proposta;
            value.etapaId = 0;
            value.situacao = "A";
            return value;
        }

        public override PropostaViewModel BeforeDelete(PropostaViewModel value)
        {
            int _esteiraId = db.Esteiras.Where(info => info.propostaId == value.propostaId).FirstOrDefault().esteiraId;
            EsteiraModel esteiraModel = new EsteiraModel(this.db, this.seguranca_db);
            EsteiraViewModel esteiraViewModel = esteiraModel.Delete(new EsteiraViewModel() { esteiraId = _esteiraId, uri = "Propostas/Delete" });
            if (esteiraViewModel.mensagem.Code > 0)
                throw new Exception(esteiraViewModel.mensagem.Message);
            return value;
        }

        public override PropostaViewModel AfterInsert(PropostaViewModel value)
        {

            #region Verifica se tem Etapa específica para o empreendimento. Se não tiver, trás a etapa "Proposta" padrão para todos os empreendimentos
            int _etapaId;
            string _descricao = DWM.Models.Enumeracoes.Enumeradores.DescricaoEtapa.PROPOSTA.GetStringValue();
            if (db.Etapas.Where(info => info.empreendimentoId == value.empreendimentoId && info.descricao == _descricao).Count() > 0)
                _etapaId = db.Etapas.Where(info => info.empreendimentoId == value.empreendimentoId && info.descricao == _descricao).FirstOrDefault().etapaId;
            else
                _etapaId = db.Etapas.Where(info => info.descricao == _descricao).FirstOrDefault().etapaId;
            #endregion

            EsteiraViewModel esteiraViewModel = new EsteiraViewModel()
            {
                propostaId = value.propostaId,
                dt_evento = Funcoes.Brasilia(),
                etapaId = _etapaId,
                dt_ocorrencia = value.dt_proposta,
                dt_manifestacao = null,
                observacao = "Inclusão de proposta. Cliente: " + db.Clientes.Find(value.clienteId).nome,
                //usuarioId = value.usuarioId,
                //nome = value.nome,
                //login = value.login,
                uri = "Propostas/Create"
            };

            EsteiraModel esteiraModel = new EsteiraModel();
            esteiraModel.Create(this.db, this.seguranca_db);

            esteiraViewModel = esteiraModel.Insert(esteiraViewModel);

            value.mensagem = esteiraViewModel.mensagem;

            return value;
        }

        public override Proposta MapToEntity(PropostaViewModel value)
        {
            Proposta proposta = Find(value);

            if (proposta == null)
            {
                proposta = new Proposta();
                value.situacao = "A";
            }

            proposta.propostaId = value.propostaId;
            proposta.empreendimentoId = value.empreendimentoId;
            proposta.clienteId = value.clienteId;
            proposta.dt_proposta = value.dt_proposta;
            proposta.unidade = value.unidade;
            proposta.torre = value.torre;
            proposta.valor = value.valor;
            proposta.vr_comissao = value.vr_comissao;
            proposta.etapaId = value.etapaId;
            proposta.dt_ultimo_status = value.dt_ultimo_status;
            proposta.operacaoId = value.operacaoId;
            proposta.corretor1Id = value.corretor1Id;
            proposta.corretor2Id = value.corretor2Id;
            proposta.usuarioId = value.usuarioId;
            proposta.nome = value.nome;
            proposta.login = value.login;
            proposta.situacao = value.situacao;

            return proposta;
        }

        public override PropostaViewModel MapToRepository(Proposta entity)
        {
            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();
            security.seguranca_db = this.seguranca_db;
            IEnumerable<GrupoRepository> grupos = security._getGrupoUsuario(sessaoCorrente.usuarioId).AsEnumerable();

            PropostaViewModel propostaViewModel = new PropostaViewModel()
            {
                propostaId = entity.propostaId,
                empreendimentoId = entity.empreendimentoId,
                descricao_empreendimento = db.Empreendimentos.Find(entity.empreendimentoId).nomeEmpreend,
                nome_coordenador = db.Empreendimentos.Find(entity.empreendimentoId).nome,
                login_coordenador = db.Empreendimentos.Find(entity.empreendimentoId).login,
                clienteId = entity.clienteId,
                nome_cliente = db.Clientes.Find(entity.clienteId).nome,
                cpf_cnpj = db.Clientes.Find(entity.clienteId).cpf_cnpj != null || db.Clientes.Find(entity.clienteId).cpf_cnpj == ""  ? Funcoes.FormataCPFCNPJ(db.Clientes.Find(entity.clienteId).cpf_cnpj) : "Não possui",
                fone1 = db.Clientes.Find(entity.clienteId).fone1 != null || db.Clientes.Find(entity.clienteId).fone1 == "" ? Funcoes.FormataTelefone(db.Clientes.Find(entity.clienteId).fone1) : "Não possui",
                fone2 = db.Clientes.Find(entity.clienteId).fone2 != null || db.Clientes.Find(entity.clienteId).fone2 == "" ? Funcoes.FormataTelefone(db.Clientes.Find(entity.clienteId).fone2) : "Não possui",
                dt_proposta = entity.dt_proposta,
                unidade = entity.unidade,
                torre = entity.torre,
                valor = entity.valor,
                vr_comissao = entity.vr_comissao,
                etapaId = entity.etapaId,
                descricao_etapa = db.Etapas.Find(entity.etapaId).descricao,
                dt_ultimo_status = entity.dt_ultimo_status,
                operacaoId = entity.operacaoId,
                corretor1Id = entity.corretor1Id,
                nome_corretor1 = entity.corretor1Id.HasValue ? db.Corretores.Find(entity.corretor1Id).nome : "",
                fone_corretor1 = entity.corretor1Id.HasValue ? db.Corretores.Find(entity.corretor1Id).fone1 : "Não possui",
                usuarioId = entity.usuarioId,
                nome = entity.nome,
                login = entity.login,
                situacao = entity.situacao,
                corretor2Id = entity.corretor2Id,
                nome_corretor2 = entity.corretor2Id.HasValue ? db.Corretores.Find(entity.corretor2Id).nome : "",
                Esteira = (from est in db.Esteiras
                           where est.propostaId == entity.propostaId
                           orderby est.esteiraId descending
                           select new EsteiraViewModel()
                           {
                               esteiraId = est.esteiraId,
                               descricao_etapa = db.Etapas.Where(info => info.etapaId == est.etapaId).FirstOrDefault().descricao,
                               propostaId = est.propostaId,
                               etapaId = est.etapaId,
                               dt_evento = est.dt_evento,
                               dt_ocorrencia = est.dt_ocorrencia,
                               dt_manifestacao = est.dt_manifestacao,
                               ind_aprovacao = est.ind_aprovacao,
                               observacao = est.observacao,
                               usuarioId = est.usuarioId,
                               nome = est.nome,
                               login = est.login
                           }).ToList(),
                Arquivos = (from arq in db.Arquivos
                            join est in db.Esteiras on arq.esteiraId equals est.esteiraId
                            where est.propostaId == entity.propostaId
                            orderby arq.arquivo
                            select new EsteiraContabilizacaoViewModel()
                            {
                                esteiraId = est.esteiraId,
                                arquivo = arq.arquivo,
                                nome_original = arq.nome_original
                            }).ToList(),
                mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso", MessageBase = "Registro incluído com sucesso", MessageType = MsgType.SUCCESS }
            };

            if (propostaViewModel.Esteira.Count() > 0)
            {
                propostaViewModel.Esteira.ElementAt(0).canApprove = (from ep in db.EtapaPerfils.AsEnumerable()
                                                                     join grp in grupos on ep.grupoId equals grp.grupoId
                                                                     where ep.etapaId == propostaViewModel.etapaId
                                                                     select ep.grupoId).Any();

                ListViewComentario list = new ListViewComentario(this.db, this.seguranca_db);
                propostaViewModel.Comentarios = list.getPagedList(0, 4, propostaViewModel.Esteira.FirstOrDefault().esteiraId);

                if ((from com in db.EsteiraComissaos
                     join est in db.Esteiras on com.esteiraId equals est.esteiraId
                     join pro in db.Propostas on est.propostaId equals pro.propostaId
                     where pro.propostaId == entity.propostaId
                           && est.ind_aprovacao == "A"
                           && com.esteiraId == (from comMax in db.EsteiraComissaos
                                                join estMax in db.Esteiras on comMax.esteiraId equals estMax.esteiraId
                                                where estMax.propostaId == entity.propostaId
                                                select comMax.esteiraId).Max()

                     select com).Count() > 0)
                {
                    ListViewEsteiraComissao listComissao = new ListViewEsteiraComissao(this.db, this.seguranca_db);
                    int? esteiraComissaoId = (from com in db.EsteiraComissaos
                                              join est in db.Esteiras on com.esteiraId equals est.esteiraId
                                              join pro in db.Propostas on est.propostaId equals pro.propostaId
                                              where pro.propostaId == entity.propostaId
                                                    && est.ind_aprovacao == "A"
                                                    && com.esteiraId == (from comMax in db.EsteiraComissaos
                                                                         join estMax in db.Esteiras on comMax.esteiraId equals estMax.esteiraId
                                                                         where estMax.propostaId == entity.propostaId
                                                                         select comMax.esteiraId).Max()
                                              select com).FirstOrDefault().esteiraId;

                    propostaViewModel.Comissao = listComissao.Bind(0, 50, esteiraComissaoId);
                }
                else
                    propostaViewModel.Comissao = new List<EsteiraComissaoViewModel>();

                if (db.Esteiras.Where(info => info.propostaId == entity.propostaId).AsEnumerable().Last().dt_manifestacao == null)
                {
                    propostaViewModel.qte_dias_esteira = (DateTime.Today.Subtract(entity.dt_proposta)).Days;
                }
                else
                {
                    System.TimeSpan diff = db.Esteiras.Where(info => info.propostaId == entity.propostaId).AsEnumerable().Last().dt_evento.Subtract(entity.dt_proposta);
                    propostaViewModel.qte_dias_esteira = diff.Days;
                }

                if (db.Etapas.Where(info => info.empreendimentoId == entity.empreendimentoId).Count() == 0)
                {
                    propostaViewModel.percent_atual = ((db.Etapas.Find(entity.etapaId).idx + 1.0) / db.Etapas.Where(info => info.empreendimentoId == null).Count()) * 100.0;
                    propostaViewModel.percent_restnte = 100.0 - propostaViewModel.percent_atual;
                }
                else
                {
                    propostaViewModel.percent_atual = ((db.Etapas.Find(entity.etapaId).idx + 1.0) / db.Etapas.Where(info => info.empreendimentoId == entity.empreendimentoId).Count()) * 100.0;
                    propostaViewModel.percent_restnte = 100.0 - propostaViewModel.percent_atual;
                }
            }

            return propostaViewModel;
        }

        public override Proposta Find(PropostaViewModel key)
        {
            return db.Propostas.Find(key.propostaId);
        }

        public override Validate Validate(PropostaViewModel value, Crud operation)
        {
            value.mensagem = new Validate() { Code = 0, Message = MensagemPadrao.Message(0).ToString() };

            if (operation == Crud.EXCLUIR || operation == Crud.ALTERAR)
            {
                if (value.propostaId <= 0)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "ID Proposta").ToString();
                    value.mensagem.MessageBase = "Identificador da proposta deve ser informado";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }

                if (operation == Crud.EXCLUIR)
                {
                    #region Verifica se tem Etapa específica para o empreendimento. Se não tiver, trás a etapa "Proposta" padrão para todos os empreendimentos
                    int _etapaId;
                    string _descricao = DWM.Models.Enumeracoes.Enumeradores.DescricaoEtapa.PROPOSTA.GetStringValue();
                    if (db.Etapas.Where(info => info.empreendimentoId == value.empreendimentoId && info.descricao == _descricao).Count() > 0)
                        _etapaId = db.Etapas.Where(info => info.empreendimentoId == value.empreendimentoId && info.descricao == _descricao).FirstOrDefault().etapaId;
                    else
                        _etapaId = db.Etapas.Where(info => info.descricao == _descricao).FirstOrDefault().etapaId;
                    #endregion

                    if (value.etapaId != _etapaId)
                    {
                        value.mensagem.Code = 16;
                        value.mensagem.Message = MensagemPadrao.Message(16).ToString();
                        value.mensagem.MessageBase = "Para realizar a exclusão, a venda precisa estar na etapa de [Proposta]. Para este caso, altere a venda e mude a situação para [Cancelada].";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                }
            }

            if (value.empreendimentoId < 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Emprendimento").ToString();
                value.mensagem.MessageBase = "Empreendimento deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.clienteId < 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Cliente").ToString();
                value.mensagem.MessageBase = "Cliente deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.torre.Trim() == "")
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Torre").ToString();
                value.mensagem.MessageBase = "Torre deve ser preenchida";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.unidade.Trim() == "")
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Unidade").ToString();
                value.mensagem.MessageBase = "Unidade deve ser preenchida";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            #region Verifica se a torre e unidade já não foram incluídas para o mesmo empreendimento
            if (operation == Crud.INCLUIR)
            {
                if (db.Propostas.Where(info => info.empreendimentoId == value.empreendimentoId &&
                                                info.torre == value.torre &&
                                                info.unidade == value.unidade).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "Já existe uma Torre e Unidade cadastrada para este empreendimento";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            else if (operation == Crud.ALTERAR)
            {
                if (db.Propostas.Where(info => info.propostaId != value.propostaId &&
                                                info.empreendimentoId == value.empreendimentoId &&
                                                info.torre == value.torre &&
                                                info.unidade == value.unidade).Count() > 0)
                {
                    value.mensagem.Code = 19;
                    value.mensagem.Message = MensagemPadrao.Message(19).ToString();
                    value.mensagem.MessageBase = "Já existe uma Torre e Unidade cadastrada para este empreendimento";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }


                #region Não permite que o valor da proposta seja alterado, caso a proposta esteja nas etapas Proposta, Analise e Reanalise
                string etapa = db.Etapas.Find(value.etapaId).descricao;
                if (etapa != DWM.Models.Enumeracoes.Enumeradores.DescricaoEtapa.PROPOSTA.GetStringValue() 
                    && etapa != DWM.Models.Enumeracoes.Enumeradores.DescricaoEtapa.ANALISE_INICIAL.GetStringValue() 
                    && etapa != DWM.Models.Enumeracoes.Enumeradores.DescricaoEtapa.REANALISE.GetStringValue())
                {
                    if (value.valor != db.Propostas.Find(value.propostaId).valor)
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(5, "Valor").ToString();
                        value.mensagem.MessageBase = "O valor não pode ser alterado nesta etapa";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                }
                #endregion

                #region Não permite que o valor da comissao seja alterado enquanto a esteira estiver até a etapa Comissao
                if (value.operacaoId >= 0)
                {
                    if (value.vr_comissao != db.Propostas.Find(value.propostaId).vr_comissao)
                    {
                        value.mensagem.Code = 5;
                        value.mensagem.Message = MensagemPadrao.Message(5, "Comissão").ToString();
                        value.mensagem.MessageBase = "O valor da comissão não pode ser alterado nesta etapa";
                        value.mensagem.MessageType = MsgType.WARNING;
                        return value.mensagem;
                    }
                }

                #endregion

                DateTime dt_ref = Funcoes.Brasilia();

                if (db.Esteiras.Where(info => info.propostaId == value.propostaId).FirstOrDefault().dt_manifestacao.HasValue)
                    dt_ref = db.Esteiras.Where(info => info.propostaId == value.propostaId).FirstOrDefault().dt_manifestacao.Value;

                if (value.dt_proposta > dt_ref)
                {
                    value.mensagem.Code = 4;
                    value.mensagem.Message = MensagemPadrao.Message(4, "Dt.Proposta", "A data da proposta deve ser menor ou igual que " + dt_ref.ToString("dd/MM/yyyy")).ToString();
                    value.mensagem.MessageBase = "A data da proposta deve ser menor ou igual a " + dt_ref.ToString("dd/MM/yyyy");
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            #endregion

            if (value.valor <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Valor geral da venda").ToString();
                value.mensagem.MessageBase = "Valor geral da venda deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.vr_comissao <= 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Valor da Comissão").ToString();
                value.mensagem.MessageBase = "Valor da comissão deve ser preenchido";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }

            if (value.etapaId < 0)
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Etapa").ToString();
                value.mensagem.MessageBase = "Etapa deve ser preenchida";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }


            #region O valor da comissão é menor que o valor da venda
            if (value.vr_comissao > (value.valor / 10))
            {
                value.mensagem.Code = 5;
                value.mensagem.Message = MensagemPadrao.Message(5, "Comissão").ToString();
                value.mensagem.MessageBase = "O valor da comissão não pode ser maior que 10% do valor total da venda";
                value.mensagem.MessageType = MsgType.WARNING;
                return value.mensagem;
            }
            #endregion

            #region a data da proposta nao pode ser maior que a menor data de aprovacao na esteira
            if (value.descricao_etapa == DWM.Models.Enumeracoes.Enumeradores.DescricaoEtapa.PROPOSTA.GetStringValue() )
            {
                if (value.dt_proposta > DateTime.Today)
                {
                    value.mensagem.Code = 5;
                    value.mensagem.Message = MensagemPadrao.Message(5, "Data da Proposta").ToString();
                    value.mensagem.MessageBase = "A data da proposta não pode ser maior que a data atual, para esta etapa.";
                    value.mensagem.MessageType = MsgType.WARNING;
                    return value.mensagem;
                }
            }
            #endregion

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
            int? _empreendimentoId = (int?)param[0];
            string _torre_unidade = (string)param[1] ?? "";
            string _cpf_nome = (string)param[2] ?? "";
            int? _etapaId = (int?)param[3];
            int? _propostaId = (int?)param[4];
            DateTime? _dt_proposta1 = (DateTime?)param[5];
            DateTime? _dt_proposta2 = (DateTime?)param[6];
            string _situacao = (string)param[7];
            int? _corretor1Id = (int?)param[8];

            if (_propostaId.HasValue) // Usuário informou o ID da proposta
                return (from p in db.Propostas
                        join c in db.Clientes on p.clienteId equals c.clienteId
                        join emp in db.Empreendimentos on p.empreendimentoId equals emp.empreendimentoId
                        join est in db.Esteiras on p.propostaId equals est.propostaId
                        join eta in db.Etapas on est.etapaId equals eta.etapaId
                        where est.esteiraId == (from esteira in db.Esteiras where esteira.propostaId == p.propostaId select esteira.esteiraId).Max()
                                && p.propostaId == _propostaId
                        orderby p.dt_proposta, c.nome
                        select new PropostaViewModel
                        {
                            empresaId = sessaoCorrente.empresaId,
                            clienteId = p.clienteId,
                            cpf_cnpj = c.cpf_cnpj,
                            nome_cliente = c.nome,
                            propostaId = p.propostaId,
                            empreendimentoId = p.empreendimentoId,
                            descricao_empreendimento = emp.nomeEmpreend,
                            dt_proposta = p.dt_proposta,
                            unidade = p.unidade,
                            torre = p.torre,
                            valor = p.valor,
                            vr_comissao = p.vr_comissao,
                            etapaId = p.etapaId,
                            descricao_etapa = eta.descricao,
                            dt_ultimo_status = p.dt_ultimo_status,
                            operacaoId = p.operacaoId,
                            percent_atual = (eta.idx + 1.0) / (from et in db.Etapas select et.etapaId).Count() * 100,
                            percent_restnte = 100 - ((eta.idx + 1.0) / (from et in db.Etapas select et.etapaId).Count()) * 100,
                            PageSize = pageSize,
                            TotalCount = (from p1 in db.Propostas
                                          join c1 in db.Clientes on p1.clienteId equals c1.clienteId
                                          join emp1 in db.Empreendimentos on p1.empreendimentoId equals emp1.empreendimentoId
                                          join est1 in db.Esteiras on p1.propostaId equals est1.propostaId
                                          join eta1 in db.Etapas on est1.etapaId equals eta1.etapaId
                                          where est1.esteiraId == (from esteira1 in db.Esteiras where esteira1.propostaId == p.propostaId select esteira1.esteiraId).Max()
                                                && p1.propostaId == _propostaId
                                          orderby p1.dt_proposta, c1.nome
                                          select p1.propostaId).Count()
                        }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
            else
                return (from p in db.Propostas
                        join c in db.Clientes on p.clienteId equals c.clienteId
                        join emp in db.Empreendimentos on p.empreendimentoId equals emp.empreendimentoId
                        join est in db.Esteiras on p.propostaId equals est.propostaId
                        join eta in db.Etapas on est.etapaId equals eta.etapaId
                        where est.esteiraId == (from esteira in db.Esteiras where esteira.propostaId == p.propostaId select esteira.esteiraId).Max()
                                && (!_empreendimentoId.HasValue || p.empreendimentoId == _empreendimentoId)
                                && (_torre_unidade == "" || (p.torre+p.unidade).Contains(_torre_unidade))
                                && (_cpf_nome == "" || c.cpf_cnpj == _cpf_nome || c.nome.Contains(_cpf_nome))
                                && (!_corretor1Id.HasValue || p.corretor1Id == _corretor1Id)
                                && (!_etapaId.HasValue || p.etapaId == _etapaId)
                                && p.dt_proposta >= _dt_proposta1 && p.dt_proposta <= _dt_proposta2
                                && p.situacao == _situacao
                        orderby p.dt_proposta, c.nome
                        select new PropostaViewModel
                        {
                            empresaId = sessaoCorrente.empresaId,
                            clienteId = p.clienteId,
                            cpf_cnpj = c.cpf_cnpj,
                            nome_cliente = c.nome,
                            propostaId = p.propostaId,
                            empreendimentoId = p.empreendimentoId,
                            descricao_empreendimento = emp.nomeEmpreend,
                            dt_proposta = p.dt_proposta,
                            unidade = p.unidade,
                            torre = p.torre,
                            valor = p.valor,
                            vr_comissao = p.vr_comissao,
                            etapaId = p.etapaId,
                            descricao_etapa = eta.descricao,
                            dt_ultimo_status = p.dt_ultimo_status,
                            operacaoId = p.operacaoId,
                            percent_atual = (eta.idx + 1.0) / (from et in db.Etapas select et.etapaId).Count() * 100,
                            percent_restnte = 100 - ((eta.idx + 1.0) / (from et in db.Etapas select et.etapaId).Count()) * 100,
                            PageSize = pageSize,
                            TotalCount = (from p1 in db.Propostas
                                          join c1 in db.Clientes on p1.clienteId equals c1.clienteId
                                          join emp1 in db.Empreendimentos on p1.empreendimentoId equals emp1.empreendimentoId
                                          join est1 in db.Esteiras on p1.propostaId equals est1.propostaId
                                          join eta1 in db.Etapas on est1.etapaId equals eta1.etapaId
                                          where est1.esteiraId == (from esteira1 in db.Esteiras where esteira1.propostaId == p.propostaId select esteira1.esteiraId).Max()
                                                && (!_empreendimentoId.HasValue || p1.empreendimentoId == _empreendimentoId)
                                                && (_torre_unidade == "" || (p1.torre + p1.unidade).Contains(_torre_unidade))
                                                && (_cpf_nome == "" || c1.cpf_cnpj == _cpf_nome || c1.nome.Contains(_cpf_nome))
                                                && (!_corretor1Id.HasValue || p1.corretor1Id == _corretor1Id)
                                                && (!_etapaId.HasValue || p1.etapaId == _etapaId)
                                                && p1.dt_proposta >= _dt_proposta1 && p1.dt_proposta <= _dt_proposta2
                                                && p1.situacao == _situacao
                                          select p1.propostaId).Count()
                        }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override string action()
        {
            return "../Home/ListPanorama";
        }

        public override string DivId()
        {
            return "div-panorama";
        }

        public override Repository getRepository(Object id)
        {
            return new PropostaModel().getObject((PropostaViewModel)id);
        }
        #endregion
    }

    public class ListViewComissaoMes : ListViewModel<PropostaViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewComissaoMes() { }
        public ListViewComissaoMes(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<PropostaViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int? _empreendimentoId = (int?)param[0];
            int? _etapaId = (int?)param[1];
            DateTime? _dt_etapa1 = (DateTime?)param[2];
            DateTime? _dt_etapa2 = (DateTime?)param[3];

            return (from p in db.Propostas
                    join c in db.Clientes on p.clienteId equals c.clienteId
                    join emp in db.Empreendimentos on p.empreendimentoId equals emp.empreendimentoId
                    join est in db.Esteiras on p.propostaId equals est.propostaId
                    join eta in db.Etapas on est.etapaId equals eta.etapaId
                    where est.esteiraId == (from esteira in db.Esteiras where esteira.propostaId == p.propostaId select esteira.esteiraId).Max()
                          && (from esteira2 in db.Esteiras 
                              where esteira2.propostaId == p.propostaId 
                                    && esteira2.etapaId == 4 
                                    && esteira2.dt_ocorrencia >= _dt_etapa1 && esteira2.dt_ocorrencia <= _dt_etapa2
                                    && esteira2.esteiraId ==  (from esteira1 in db.Esteiras
                                                               where esteira1.propostaId == p.propostaId && esteira1.etapaId == 4 && esteira1.dt_ocorrencia >= _dt_etapa1 && esteira1.dt_ocorrencia <= _dt_etapa2
                                                               select esteira1.esteiraId).Max()
                              select esteira2.esteiraId).Count() > 0
                          && (!_empreendimentoId.HasValue || p.empreendimentoId == _empreendimentoId)
                          && p.etapaId >= _etapaId
                          && p.situacao == "A"
                    orderby p.dt_ultimo_status, c.nome
                    select new PropostaViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        clienteId = p.clienteId,
                        cpf_cnpj = c.cpf_cnpj,
                        nome_cliente = c.nome,
                        propostaId = p.propostaId,
                        empreendimentoId = p.empreendimentoId,
                        descricao_empreendimento = emp.nomeEmpreend,
                        dt_proposta = p.dt_proposta,
                        unidade = p.unidade,
                        torre = p.torre,
                        valor = p.valor,
                        vr_comissao = p.vr_comissao,
                        etapaId = p.etapaId,
                        descricao_etapa = eta.descricao,
                        dt_ultimo_status = p.dt_ultimo_status,
                        operacaoId = p.operacaoId,
                        percent_atual = (eta.idx + 1.0) / (from et in db.Etapas select et.etapaId).Count() * 100,
                        percent_restnte = 100 - ((eta.idx + 1.0) / (from et in db.Etapas select et.etapaId).Count()) * 100,
                        PageSize = pageSize,
                        TotalCount = (from p1 in db.Propostas
                                      join c1 in db.Clientes on p1.clienteId equals c1.clienteId
                                      join emp1 in db.Empreendimentos on p1.empreendimentoId equals emp1.empreendimentoId
                                      join est1 in db.Esteiras on p1.propostaId equals est1.propostaId
                                      join eta1 in db.Etapas on est1.etapaId equals eta1.etapaId
                                      where est1.esteiraId == (from esteira3 in db.Esteiras where esteira3.propostaId == p1.propostaId select esteira3.esteiraId).Max()
                                              && (from esteira21 in db.Esteiras
                                                  where esteira21.propostaId == p1.propostaId
                                                        && esteira21.etapaId == 4
                                                        && esteira21.dt_ocorrencia >= _dt_etapa1 && esteira21.dt_ocorrencia <= _dt_etapa2
                                                        && esteira21.esteiraId == (from esteira11 in db.Esteiras
                                                                                  where esteira11.propostaId == p1.propostaId && esteira11.etapaId == 4 && esteira11.dt_ocorrencia >= _dt_etapa1 && esteira11.dt_ocorrencia <= _dt_etapa2
                                                                                  select esteira11.esteiraId).Max()
                                                  select esteira21.esteiraId).Count() > 0
                                              && (!_empreendimentoId.HasValue || p1.empreendimentoId == _empreendimentoId)
                                              && p1.etapaId >= _etapaId
                                              && p1.situacao == "A"
                                      orderby p1.dt_ultimo_status, c1.nome
                                      select p1.propostaId).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class ListViewVendasEmAberto : ListViewModel<PropostaViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewVendasEmAberto() { }
        public ListViewVendasEmAberto(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<PropostaViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int? _empreendimentoId = (int?)param[0];
            int? _etapaId = (int?)param[1];

            return (from p in db.Propostas
                    join c in db.Clientes on p.clienteId equals c.clienteId
                    join emp in db.Empreendimentos on p.empreendimentoId equals emp.empreendimentoId
                    join est in db.Esteiras on p.propostaId equals est.propostaId
                    join eta in db.Etapas on est.etapaId equals eta.etapaId
                    where est.esteiraId == (from esteira in db.Esteiras where esteira.propostaId == p.propostaId select esteira.esteiraId).Max()
                          && (!_empreendimentoId.HasValue || p.empreendimentoId == _empreendimentoId)
                          && p.etapaId <= _etapaId
                          && p.situacao == "A"
                    orderby p.dt_ultimo_status descending, c.nome
                    select new PropostaViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        clienteId = p.clienteId,
                        cpf_cnpj = c.cpf_cnpj,
                        nome_cliente = c.nome,
                        propostaId = p.propostaId,
                        empreendimentoId = p.empreendimentoId,
                        descricao_empreendimento = emp.nomeEmpreend,
                        dt_proposta = p.dt_proposta,
                        unidade = p.unidade,
                        torre = p.torre,
                        valor = p.valor,
                        vr_comissao = p.vr_comissao,
                        etapaId = p.etapaId,
                        descricao_etapa = eta.descricao,
                        dt_ultimo_status = p.dt_ultimo_status,
                        operacaoId = p.operacaoId,
                        percent_atual = (eta.idx + 1.0) / (from et in db.Etapas select et.etapaId).Count() * 100,
                        percent_restnte = 100 - ((eta.idx + 1.0) / (from et in db.Etapas select et.etapaId).Count()) * 100,
                        PageSize = pageSize,
                        TotalCount = (from p1 in db.Propostas
                                      join c1 in db.Clientes on p1.clienteId equals c1.clienteId
                                      join emp1 in db.Empreendimentos on p1.empreendimentoId equals emp1.empreendimentoId
                                      join est1 in db.Esteiras on p1.propostaId equals est1.propostaId
                                      join eta1 in db.Etapas on est1.etapaId equals eta1.etapaId
                                      where est1.esteiraId == (from esteira1 in db.Esteiras where esteira1.propostaId == p1.propostaId select esteira1.esteiraId).Max()
                                              && (!_empreendimentoId.HasValue || p1.empreendimentoId == _empreendimentoId)
                                              && p1.etapaId <= _etapaId
                                              && p1.situacao == "A"
                                      orderby p1.dt_ultimo_status descending, c1.nome
                                      select p1.propostaId).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class ListViewVendasAtrasadas : ListViewModel<PropostaViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewVendasAtrasadas() { }
        public ListViewVendasAtrasadas(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<PropostaViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int? _empreendimentoId = null;
            var _data = DateTime.Today.AddDays(-10);

            if (param != null)
                if (param[0] != null)
                    _empreendimentoId = (int?)param[0];

            return (from p in db.Propostas
                    join c in db.Clientes on p.clienteId equals c.clienteId
                    join emp in db.Empreendimentos on p.empreendimentoId equals emp.empreendimentoId
                    join est in db.Esteiras on p.propostaId equals est.propostaId
                    join eta in db.Etapas on est.etapaId equals eta.etapaId
                    where est.esteiraId == (from esteira in db.Esteiras where esteira.propostaId == p.propostaId select esteira.esteiraId).Max()
                          && (!_empreendimentoId.HasValue || p.empreendimentoId == _empreendimentoId)
                          && p.situacao == "A"
                          && p.dt_ultimo_status < _data
                    orderby p.dt_ultimo_status descending, c.nome
                    select new PropostaViewModel
                    {
                        empresaId = sessaoCorrente.empresaId,
                        clienteId = p.clienteId,
                        cpf_cnpj = c.cpf_cnpj,
                        nome_cliente = c.nome,
                        propostaId = p.propostaId,
                        empreendimentoId = p.empreendimentoId,
                        descricao_empreendimento = emp.nomeEmpreend,
                        dt_proposta = p.dt_proposta,
                        unidade = p.unidade,
                        torre = p.torre,
                        valor = p.valor,
                        vr_comissao = p.vr_comissao,
                        etapaId = p.etapaId,
                        descricao_etapa = eta.descricao,
                        dt_ultimo_status = p.dt_ultimo_status,
                        operacaoId = p.operacaoId,
                        percent_atual = (eta.idx + 1.0) / (from et in db.Etapas select et.etapaId).Count() * 100,
                        percent_restnte = 100 - ((eta.idx + 1.0) / (from et in db.Etapas select et.etapaId).Count()) * 100,
                        PageSize = pageSize,
                        TotalCount = (from p1 in db.Propostas
                                      join c1 in db.Clientes on p1.clienteId equals c1.clienteId
                                      join emp1 in db.Empreendimentos on p1.empreendimentoId equals emp1.empreendimentoId
                                      join est1 in db.Esteiras on p1.propostaId equals est1.propostaId
                                      join eta1 in db.Etapas on est1.etapaId equals eta1.etapaId
                                      where est1.esteiraId == (from esteira1 in db.Esteiras where esteira1.propostaId == p1.propostaId select esteira1.esteiraId).Max()
                                              && (!_empreendimentoId.HasValue || p1.empreendimentoId == _empreendimentoId)
                                              && p1.situacao == "A"
                                              && p1.dt_ultimo_status < _data
                                      orderby p1.dt_ultimo_status descending, c1.nome
                                      select p1.propostaId).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    public class ListViewResumoVenda : ListViewModel<ResumoVendaViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewResumoVenda() { }
        public ListViewResumoVenda(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<ResumoVendaViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int? _empreendimentoId = null;
            DateTime? _dt_proposta1 = new DateTime(1980, 1, 1);
            DateTime? _dt_proposta2 = new DateTime(2030, 12, 31);

            if (param != null)
            {
                if (param[0] != null)
                    _empreendimentoId = (int?)param[0];
                if (param [1] != null)
                    _dt_proposta1 = (DateTime?)param[1];
                if (param [2] != null)
                    _dt_proposta2 = (DateTime?)param[2];
            }

            return (from pro in db.Propostas 
                    join emp in db.Empreendimentos on pro.empreendimentoId equals emp.empreendimentoId
                    where (!_empreendimentoId.HasValue || pro.empreendimentoId == _empreendimentoId )
                            && pro.dt_proposta >= _dt_proposta1 && pro.dt_proposta <= _dt_proposta2
                            && pro.situacao == "A" && pro.etapaId >= 4
                    group pro by new {pro.empreendimentoId, emp.nomeEmpreend} into PRO
                    select new ResumoVendaViewModel
                    {
                        empreendimentoId = PRO.Key.empreendimentoId, 
                        nome_empreendimento = PRO.Key.nomeEmpreend, 
                        vgv_areceber = PRO.Sum(pro => pro.valor), 
                        vgv_recebido = 0,
                        total_comissao = PRO.Sum(pro => pro.vr_comissao), 
                        descricao_etapa = "",
                        quantidade = PRO.Count(),
                        PageSize = pageSize,
                        TotalCount = (from pro1 in db.Propostas
                                      join emp1 in db.Empreendimentos on pro1.empreendimentoId equals emp1.empreendimentoId
                                      where (!_empreendimentoId.HasValue || pro1.empreendimentoId == _empreendimentoId)
                                              && pro1.dt_proposta >= _dt_proposta1 && pro1.dt_proposta <= _dt_proposta2
                                              && pro1.situacao == "A" && pro1.etapaId >= 4
                                      group pro1 by new { pro1.empreendimentoId, emp1.nomeEmpreend } into PRO1
                                      select PRO.Key).Count()
                    }).OrderBy(info => info.nome_empreendimento).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}