using System;
using System.Collections.Generic;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using DWM.Models.Persistence;
using System.Web.Mvc;
using System.Linq;

namespace DWM.Models.BI
{
    public class CheckPropostaBI : DWMContext<ApplicationContext>, IProcess<PropostaViewModel, ApplicationContext>
    {
         #region Constructor
        public CheckPropostaBI() { }

        public CheckPropostaBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        public virtual PropostaViewModel Run(Repository value)
        {
            PropostaViewModel r = (PropostaViewModel)value;
            r.mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso!!" };
            try
            {
                ListViewProposta modelPanorama = new ListViewProposta(this.db, this.seguranca_db);
                IEnumerable<PropostaViewModel> listProposta = modelPanorama.Bind(0, 15, r.empreendimentoId, null, null, null, r.propostaId, DateTime.Today, DateTime.Today, null, null);
                if (listProposta == null)
                    throw new ArgumentException();
                else if (listProposta.Count() == 0)
                    throw new ArgumentException();
                else
                {
                    r = listProposta.FirstOrDefault();
                    r.mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso!!" };
                }
            }
            catch (ArgumentException ex)
            {
                r.mensagem = new Validate() { Code = 999, MessageBase = ex.Message, Message = "Usuário não autorizado a acessar esta função" };
            }
            catch (Exception ex)
            {
                r.mensagem = new Validate() { Code = 999, MessageBase = ex.Message, Message = "Usuário não autorizado a acessar esta função" };
            }
            return r;
        }

        public IEnumerable<PropostaViewModel> List(params object[] param)
        {
            throw new NotImplementedException(); 
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException(); 
        }
    }
}