﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IndividueelProject
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MagazijnEntities : DbContext
    {
        public MagazijnEntities()
            : base("name=MagazijnEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bestelling> Bestelling { get; set; }
        public virtual DbSet<BestellingProduct> BestellingProduct { get; set; }
        public virtual DbSet<Categorie> Categorie { get; set; }
        public virtual DbSet<Klant> Klant { get; set; }
        public virtual DbSet<Leverancier> Leverancier { get; set; }
        public virtual DbSet<Personeelslid> Personeelslid { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<Subcategorie> Subcategorie { get; set; }
    }
}
