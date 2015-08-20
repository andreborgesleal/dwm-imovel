using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace DWM.Models.Repositories
{
    public class HomeViewModel : Repository
    {
        public IPagedList PropostaViewModel { get; set; }
        public IPagedList Comentarios { get; set; }
    }
}