using System;
using System.Collections.Generic;
using System.Linq;
using App_Dominio.Contratos;
using App_Dominio.Entidades;
using App_Dominio.Component;
using DWM.Models.Repositories;
using DWM.Models.Entidades;
using App_Dominio.Enumeracoes;
using App_Dominio.Models;
using System.IO;
using System.Web;
using DWM.Models.Persistence;

namespace DWM.Models.BI
{
    public class InserirComentarioBI : DWMContext<ApplicationContext>, IProcess<EsteiraComentarioViewModel, ApplicationContext>
    {
                #region Constructor
        public InserirComentarioBI() { }

        public InserirComentarioBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        public EsteiraComentarioViewModel Run(Repository value)
        {
            EsteiraComentarioViewModel r = (EsteiraComentarioViewModel)value;
            try
            {
                EsteiraComentarioModel model = new EsteiraComentarioModel(this.db, this.seguranca_db);
                r = model.Insert(r);
            }
            catch(Exception ex)
            {
                r.mensagem = new Validate() { Code = 999, MessageBase = ex.Message, Message = "Ocorreu um erro na inclusão do comentário" };
            }
            return r;
        }

        public IEnumerable<EsteiraComentarioViewModel> List(params object[] param)
        {
            ListViewComentario list = new ListViewComentario(this.db, this.seguranca_db);
            return list.Bind(0, 100, param);
        }

    }
}