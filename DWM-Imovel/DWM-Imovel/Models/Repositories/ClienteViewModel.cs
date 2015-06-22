using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App_Dominio.Component;
using System;
using System.Collections.Generic;
using DWM.Models.Entidades;

namespace DWM.Models.Repositories
{
    public class ClienteViewModel : Repository
    {
        [Key]
        [DisplayName("ID")]
        public int clienteId { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Por favor, informe o nome do cliente")]
        [StringLength(50, ErrorMessage = "O nome do cliente deve ter no máximo 50 caracteres")]
        public string nome { get; set; }

        [DisplayName("CPF/CPNJ")]
        [Required(ErrorMessage = "Por favor, informe o cpf/cnpj do cliente")]
        [StringLength(14, ErrorMessage = "O cpf/cnpj deve ter no máximo 14 caracteres")]
        public string cpf_cnpj { get; set; }

        [DisplayName("Telefone")]
        public string telefone { get; set; }
    }
}