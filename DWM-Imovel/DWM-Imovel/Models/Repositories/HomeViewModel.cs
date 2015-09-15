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
        public int? empreendimentoId { get; set; }
        public string torre_unidade { get; set; }
        public string cpf_nome { get; set; }
        public int? etapaId { get; set; }
        public int? propostaId { get; set; }
        public DateTime? dt_proposta1 { get; set; }
        public DateTime? dt_proposta2 { get; set; }
        public string situacao { get; set; }
        public int? corretor1Id { get; set; }

        public IPagedList Panorama { get; set; }
        public IPagedList UltimosComentarios { get; set; }
        public IEnumerable<PropostaViewModel> ComissaoMes { get; set; }
        public IEnumerable<PropostaViewModel> VendasMes { get; set; }
        public IEnumerable<PropostaViewModel> VendasEmAberto{ get; set; }
        public IEnumerable<PropostaViewModel> VendasEmAtraso { get; set; }
        public IEnumerable<ResumoVendaViewModel> ResumoVenda { get; set; }

    }
}