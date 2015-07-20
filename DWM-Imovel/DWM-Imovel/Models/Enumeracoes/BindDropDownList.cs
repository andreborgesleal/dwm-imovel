using App_Dominio.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App_Dominio.Entidades;
//using System.Data.Objects.SqlClient;
using DWM.Models.Entidades;
using App_Dominio.Security;
using App_Dominio.Models;
using App_Dominio.App_Start;

namespace DWM.Models.Enumeracoes
{
    public class BindDropDownList
    {
        public IEnumerable<SelectListItem> Usuarios(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            EmpresaSecurity<SecurityContext> security = new EmpresaSecurity<SecurityContext>();

            Sessao s = security.getSessaoCorrente();
            IEnumerable<App_Dominio.Repositories.UsuarioRepository> Usuarios = security.getUsuarios((int)Sistema.DWMIMOVEL, s.empresaId);

            IList<SelectListItem> q = new List<SelectListItem>();

            if (cabecalho != "")
                q.Add(new SelectListItem() { Value = "", Text = cabecalho });

            q = q.Union(from e in Usuarios.AsEnumerable()
                        orderby e.nome
                        select new SelectListItem()
                        {
                            Value = e.usuarioId.ToString(),
                            Text = e.nome,
                            Selected = (selectedValue != "" ? e.nome.Equals(selectedValue) : false)
                        }).ToList();

            return q;
        }

        public IEnumerable<SelectListItem> Etapas(params object[] param)
        {
            // params[0] -> cabeçalho (Selecione..., Todos...)
            // params[1] -> SelectedValue
            string cabecalho = param[0].ToString();
            string selectedValue = param[1].ToString();

            using (ApplicationContext db = new ApplicationContext())
            {
                IList<SelectListItem> q = new List<SelectListItem>();

                if (cabecalho != "")
                    q.Add(new SelectListItem() { Value = "", Text = cabecalho });

                q = q.Union(from e in db.Etapas.AsEnumerable()
                            where e.empreendimentoId == null
                            orderby e.etapaId
                            select new SelectListItem()
                            {
                                Value = e.etapaId.ToString(),
                                Text = e.descricao,
                                Selected = (selectedValue != "" ? e.descricao.Equals(selectedValue) : false)
                            }).ToList();

                return q;
            }
        }
    }
}