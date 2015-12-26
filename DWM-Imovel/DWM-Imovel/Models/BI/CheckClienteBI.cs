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
    public class CheckClienteBI : DWMContext<ApplicationContext>, IProcess<ClienteViewModel, ApplicationContext>
    {
        #region Constructor
        public CheckClienteBI() { }

        public CheckClienteBI(ApplicationContext _db, SecurityContext _seguranca_db)
        {
            base.Create(_db, _seguranca_db);
        }
        #endregion

        public virtual ClienteViewModel Run(Repository value)
        {
            ClienteViewModel r = (ClienteViewModel)value;
            r.mensagem = new Validate() { Code = 0, Message = "Registro incluído com sucesso!!" };
            try
            {
                ListViewCliente model = new ListViewCliente(this.db, this.seguranca_db);
                IEnumerable<ClienteViewModel> listClientes = model.Bind(0, 15, null, r.clienteId);
                if (listClientes == null)
                    throw new ArgumentException();
                else if (listClientes.Count() == 0)
                    throw new ArgumentException();
                else
                {
                    r = listClientes.FirstOrDefault();
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

        public IEnumerable<ClienteViewModel> List(params object[] param)
        {
            throw new NotImplementedException();
        }

        public IPagedList PagedList(int? index, int pageSize = 50, params object[] param)
        {
            throw new NotImplementedException();
        }
    }
}