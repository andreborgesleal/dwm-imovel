using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App_Dominio.Entidades;
using App_Dominio.Component;
using App_Dominio.Contratos;
using System.Data.Entity;
using DWM.Models.Repositories;

namespace DWM.Facade
{
    public class ApplicationFacade<R, T, D> : Facade<R, T, D>, IFacade<R, D>
        where R : Repository
        where T : ICrudModel<R, D>
        where D : DbContext
    {
        public R Run(IProcess<R, D> proc, Repository value = null)
        {
            using (db = getContextInstance())
            {
                using (seguranca_db = new SecurityContext())
                {
                    proc.Create(db, seguranca_db);
                    return proc.Run(value);
                }
            }
        }

        public IEnumerable<R> List(IProcess<R, D> proc, params object[] param)
        {
            using (db = getContextInstance())
            {
                using (seguranca_db = new SecurityContext())
                {
                    proc.Create(db, seguranca_db);
                    return proc.List(param);
                }
            }
        }
    }
}