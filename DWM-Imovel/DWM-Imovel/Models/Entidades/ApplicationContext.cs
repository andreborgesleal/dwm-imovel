﻿using App_Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DWM.Models.Entidades
{
    public class ApplicationContext : App_DominioContext
    {
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Empreendimento> Empreendimentos { get; set; }
        public DbSet<Corretor> Corretores { get; set; }
        public DbSet<Proposta> Propostas { get; set; }
        public DbSet<Esteira> Esteiras { get; set; }
        public DbSet<Etapa> Etapas { get; set;  }
        public DbSet<EsteiraComentario> Comentarios { get; set; }
        public DbSet<EsteiraContabilizacao> Arquivos { get; set; }
        public DbSet<EtapaPerfil> EtapaPerfils { get; set; }
        public DbSet<EsteiraComissao> EsteiraComissaos { get; set; }
        public DbSet<ComissaoDefault> ComissaoDefaults { get; set; }

    }
}
