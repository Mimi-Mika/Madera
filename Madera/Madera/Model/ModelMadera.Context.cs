﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Madera.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBEntities : DbContext
    {
        public DBEntities()
            : base("name=DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Commercial> Commercial { get; set; }
        public virtual DbSet<Couleur> Couleur { get; set; }
        public virtual DbSet<Empreinte> Empreinte { get; set; }
        public virtual DbSet<EtatCommande> EtatCommande { get; set; }
        public virtual DbSet<Favori> Favori { get; set; }
        public virtual DbSet<Finition> Finition { get; set; }
        public virtual DbSet<Gamme> Gamme { get; set; }
        public virtual DbSet<Maison> Maison { get; set; }
        public virtual DbSet<Maison_TypeDalle> Maison_TypeDalle { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<Module_Favori> Module_Favori { get; set; }
        public virtual DbSet<Module_Maison> Module_Maison { get; set; }
        public virtual DbSet<Projet> Projet { get; set; }
        public virtual DbSet<Projet_EtatCommande> Projet_EtatCommande { get; set; }
        public virtual DbSet<TypeDalle> TypeDalle { get; set; }
        public virtual DbSet<TypeModule> TypeModule { get; set; }
        public virtual DbSet<ZoneMorte> ZoneMorte { get; set; }
    }
}
