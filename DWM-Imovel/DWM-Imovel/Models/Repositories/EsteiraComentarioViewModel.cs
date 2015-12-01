using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;
using App_Dominio.Models;

namespace DWM.Models.Repositories
{
    public class EsteiraComentarioViewModel : Repository
    {
        [DisplayName("ID")]
        public int esteiraId { get; set; }

        public int? propostaId { get; set; }

        [DisplayName("Data")]
        public DateTime dt_comentario { get; set; }

        [DisplayName("Etapa")]
        public string descricao_etapa { get; set; }

        [DisplayName("Indicação de Aprovação")]
        public string ind_aprovacao { get; set; }

        [DisplayName("Observação")]
        [Required(ErrorMessage="Campo Observação deve ser preenchido")]
        [StringLength(4000, ErrorMessage="Campo Observação deve ter no máximo 4.000 caracteres")]
        public string observacao { get; set; }

        [DisplayName("Usuário")]
        public int usuarioId { get; set; }

        [DisplayName("Nome")]
        public string nome { get; set; }

        [DisplayName("Login")]
        public string login { get; set; }

        public string nome_cliente { get; set; }

        public string nome_empreendimento { get; set; }

        [DisplayName("Tempo Comentário")]
        public string tempo_comentario { 
            get 
            {
                string value = "há ";
                TimeSpan ts = Funcoes.Brasilia().Subtract(dt_comentario);
                if (ts.Days > 0)
                    value += ts.Days.ToString() + " dias";
                else if (ts.Hours > 0)
                    value += ts.Hours.ToString() + " horas";
                else if (ts.Minutes > 0)
                    value += ts.Minutes.ToString() + " min.";
                else
                    value += ts.Seconds.ToString() + " seg.";

                return value;
            }
        }
    }
}