using System.Collections.Generic;
using App_Dominio.Entidades;
using App_Dominio.Component;
using App_Dominio.Contratos;
using System.Data.Entity;
using App_Dominio.Pattern;

namespace DWM.Facade
{
    public class ApplicationFactory<R, D> : Factory<R, D>, IProcess<R, D>
        where R : Repository
        //where T : ICrudModel<R, D>
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

        public R Run(IProcess<R, D> proc, Repository value = null, params object[] param)
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