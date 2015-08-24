using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;
using App_Dominio.Models;

namespace DWM.Models.Repositories
{
    public class EsteiraViewModel : Repository
    {
        [DisplayName("ID")]
        public int esteiraId { get; set; }

        [DisplayName("PropostaID")]
        public int propostaId { get; set; }

        [DisplayName("Dt.Evento")]
        public DateTime dt_evento { get; set; }

        [DisplayName("EtapaID")]
        public int etapaId { get; set; }

        [DisplayName("Etapa")]
        public string descricao_etapa { get; set; }

        [DisplayName("Dt.Ocorrência")]
        public DateTime dt_ocorrencia { get; set; }

        [DisplayName("Dt.Manifestação")]
        public Nullable<DateTime> dt_manifestacao { get; set; }

        [DisplayName("Aprovação")]
        public string ind_aprovacao { get; set; }

        [DisplayName("Observação")]
        public string observacao { get; set; }

        [DisplayName("UsuarioId")]
        public Nullable<int> usuarioId { get; set; }

        [DisplayName("Nome")]
        public string nome { get; set; }

        [DisplayName("Login")]
        public string login { get; set; }

        [DisplayName("Tempo")]
        public string tempo_etapa {
            get
            {
                string value = "há ";
                TimeSpan ts = Funcoes.Brasilia().Subtract(dt_ocorrencia);
                if (ts.Days > 0)
                    value += ts.Days.ToString() + " dias";
                else if (ts.Hours > 0)
                    value += ts.Hours.ToString() + " horas";
                else if (ts.Minutes > 0)
                    value += ts.Minutes.ToString() + " min";
                else
                    value += ts.Seconds.ToString() + " seg";

                return value;
            }
        }


        public bool canApprove { get; set; }

        public virtual PropostaViewModel proposta { get; set; }

    }
}