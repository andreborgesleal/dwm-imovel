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
        public override EsteiraViewModel BeforeInsert(EsteiraViewModel value)
        {
            value.dt_evento = Funcoes.Brasilia();

            return value;
        }

        public override EsteiraViewModel BeforeUpdate(EsteiraViewModel value)
        {
            Usuario u = seguranca_db.Usuarios.Find(sessaoCorrente.usuarioId);

            value.usuarioId = sessaoCorrente.usuarioId;
            value.nome = u.nome;
            value.login = u.login;

            return value;
        }

        public override Esteira MapToEntity(EsteiraViewModel value)
        {
            Esteira est = Find(value);

            if (est == null)
                est = new Esteira();

            est.propostaId = value.propostaId;
            est.dt_evento = value.dt_evento;
            est.etapaId = value.etapaId;
            est.dt_ocorrencia = value.dt_ocorrencia;
            est.ind_aprovacao = value.ind_aprovacao;
            est.observacao = value.observacao;
            est.dt_manifestacao = value.dt_manifestacao;
            est.usuarioId = value.usuarioId;
            est.nome = value.nome;
            est.login = value.login;

            return est;
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

    public class ListViewEsteira : ListViewModel<EsteiraViewModel, ApplicationContext>
    {
        #region Constructor
        public ListViewEsteira() { }
        public ListViewEsteira(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        #region Métodos da classe ListViewRepository
        public override IEnumerable<EsteiraViewModel> Bind(int? index, int pageSize = 50, params object[] param)
        {
            int _propostaId = int.Parse(param[0].ToString());

            return (from est in db.Esteiras
                    where est.propostaId == _propostaId
                    orderby est.etapaId descending
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
                        login = est.login,
                        PageSize = pageSize,
                        TotalCount = (from est1 in db.Esteiras
                                      where est1.propostaId == _propostaId
                                      orderby est1.etapaId descending
                                      select est1.esteiraId).Count()
                    }).Skip((index ?? 0) * pageSize).Take(pageSize).ToList();
        }

        public override Repository getRepository(Object id)
        {
            return new EsteiraModel().getObject((EsteiraViewModel)id);
        }

        public override string action()
        {
            return "../Workflow/ListEsteira";
        }

        public override string DivId()
        {
            return "esteira";
        }

        #endregion
    }
}