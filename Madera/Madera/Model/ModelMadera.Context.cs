﻿

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
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

    public virtual DbSet<Empreinte> Empreinte { get; set; }

    public virtual DbSet<EtatCommande> EtatCommande { get; set; }

    public virtual DbSet<Favori> Favori { get; set; }

    public virtual DbSet<Maison> Maison { get; set; }

    public virtual DbSet<Maison_TypeDalle> Maison_TypeDalle { get; set; }

    public virtual DbSet<Module> Module { get; set; }

    public virtual DbSet<Module_Favori> Module_Favori { get; set; }

    public virtual DbSet<Projet_EtatCommande> Projet_EtatCommande { get; set; }

    public virtual DbSet<TypeDalle> TypeDalle { get; set; }

    public virtual DbSet<TypeModule> TypeModule { get; set; }

    public virtual DbSet<ZoneMorte> ZoneMorte { get; set; }

    public virtual DbSet<Couleur> Couleur { get; set; }

    public virtual DbSet<Module_Maison> Module_Maison { get; set; }

    public virtual DbSet<Projet> Projet { get; set; }

    public virtual DbSet<Gamme> Gamme { get; set; }

}

}

