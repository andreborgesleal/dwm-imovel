﻿using App_Dominio.Entidades;
using App_Dominio.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using System.Globalization;
using System.Text;
using System.Threading;

namespace DWM
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            //CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-BR");
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;

            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void Session_Start(object sender, EventArgs e)
        {
            HttpContext.Current.Session.Add("__MyAppSession", string.Empty);

            //CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-BR");
            //Thread.CurrentThread.CurrentCulture = culture;
            //Thread.CurrentThread.CurrentUICulture = culture;

        }

        public void Session_End(object sender, EventArgs e)
        {
            EmpresaSecurity<App_DominioContext> login = new EmpresaSecurity<App_DominioContext>();
            if (System.Web.HttpContext.Current != null)
                login.EncerrarSessao(System.Web.HttpContext.Current.Session.SessionID);
        }

        protected void Application_End()
        {
            EmpresaSecurity<App_DominioContext> login = new EmpresaSecurity<App_DominioContext>();
            if (System.Web.HttpContext.Current != null)
                login.EncerrarSessao(System.Web.HttpContext.Current.Session.SessionID);
        }
    }
}
