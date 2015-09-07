using App_Dominio.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DWM.Models.Enumeracoes
{
    public class Enumeradores
    {
        public enum Param
        {
            //GRUPO_USUARIO = 1,
            //SISTEMA = 2,
            //EMPRESA = 3,
            HABILITA_EMAIL = 6,
            FUSO_HORARIO = 7
        }

        public enum DescricaoEtapa
        {
            [StringDescription("Proposta")]
            [StringValue("Proposta")]
            PROPOSTA,
            [StringDescription("Análise Inicial")]
            [StringValue("Análise Inicial")]
            ANALISE_INICIAL,
            [StringDescription("Reanálise")]
            [StringValue("Reanálise")]
            REANALISE,
            [StringDescription("Análise de Crédito")]
            [StringValue("Análise de Crédito")]
            ANALISE_CREDITO,
            [StringDescription("Contabilização")]
            [StringValue("Contabilização")]
            CONTABILIZACAO,
            [StringDescription("Comissão")]
            [StringValue("Comissão")]
            COMISSAO,
            [StringDescription("Faturamento")]
            [StringValue("Faturamento")]
            FATURAMENTO,
        }
    }

}

