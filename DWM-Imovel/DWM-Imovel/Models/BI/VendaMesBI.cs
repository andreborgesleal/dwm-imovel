using System;
using System.Collections.Generic;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using DWM.Models.Persistence;
using System.Web.Mvc;

namespace DWM.Models.BI
{
    public class VendaMesBI : DWMContext<ApplicationContext>, IProcess<HomeViewModel, ApplicationContext>
    {
        #region Constructor
        public VendaMesBI() { }

        public VendaMesBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        public HomeViewModel Run(Repository value)
        {
            HomeViewModel r = (HomeViewModel)value;
            try
            {
                DateTime _dt_prop1 = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM-") + "01");
                DateTime _dt_prop2 = DateTime.Today;

                #region Vendas do mês (todas)
                ListViewProposta modelVendas = new ListViewProposta(this.db, this.seguranca_db);
                r.VendasMes = modelVendas.Bind(0, 1000, null, null, null, null, null, _dt_prop1, _dt_prop2, "A", null);
                #endregion


                //#region Resumo da Venda
                //ListViewResumoVenda modelResumoVenda = new ListViewResumoVenda(this.db, this.seguranca_db);
                //r.ResumoVenda = modelResumoVenda.Bind(0, 1000, null, null, null);
                //#endregion

                /*
                #region Comissão do mÊs (somente das vendas com análise de crédito aprovada dentro do mês corrente)
                ListViewComissaoMes modelComissaoMes = new ListViewComissaoMes(this.db, this.seguranca_db);
                r.ComissaoMes = modelComissaoMes.Bind(0, 1000, null, 4, _dt_prop1, _dt_prop2);
                #endregion

                #region Vendas do mês (todas)
                ListViewProposta modelVendas = new ListViewProposta(this.db, this.seguranca_db);
                r.VendasMes = modelVendas.Bind(0, 1000, null, null, null, null, null, _dt_prop1, _dt_prop2, "A", null);
                #endregion

                #region Vendas em aberto
                ListViewVendasEmAberto modelAberto = new ListViewVendasEmAberto(this.db, this.seguranca_db);
                r.VendasEmAberto = null; // modelAberto.Bind(0, 1000, null, 3);
                #endregion

                #region Vendas atrasadas
                ListViewVendasAtrasadas modelAtraso = new ListViewVendasAtrasadas(this.db, this.seguranca_db);
                r.VendasEmAtraso = null; // modelAtraso.Bind(0, 1000, null);
                #endregion

                #region Resumo da Venda
                ListViewResumoVenda modelResumoVenda = new ListViewResumoVenda(this.db, this.seguranca_db);
                r.ResumoVenda = modelResumoVenda.Bind(0, 1000, null, null, null);
                #endregion

                */
            }
            catch (Exception ex)
            {
                r.mensagem = new Validate() { Code = 999, MessageBase = ex.Message, Message = "Ocorreu um erro na recuperação dos dados" };
            }
            return r;
        }

        public IEnumerable<HomeViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }
    }
}