﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Work.WebApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RenHai2012Entities : DbContext
    {
        public RenHai2012Entities()
            : base("name=RenHai2012Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Orders_Detail> Orders_Detail { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Member_Detail> Member_Detail { get; set; }
        public virtual DbSet<Light_Site> Light_Site { get; set; }
        public virtual DbSet<Manjushri> Manjushri { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<Member_Detail_Category> Member_Detail_Category { get; set; }
        public virtual DbSet<Reject> Reject { get; set; }
        public virtual DbSet<Reject_Detail> Reject_Detail { get; set; }
        public virtual DbSet<i_IDX> i_IDX { get; set; }
        public virtual DbSet<Fortune_Light> Fortune_Light { get; set; }
        public virtual DbSet<TempleMember> TempleMember { get; set; }
        public virtual DbSet<TempleAccount> TempleAccount { get; set; }
        public virtual DbSet<AssemblyBatch> AssemblyBatch { get; set; }
        public virtual DbSet<Wish> Wish { get; set; }
        public virtual DbSet<Wish_Light> Wish_Light { get; set; }
        public virtual DbSet<AssemblyBatchChglog> AssemblyBatchChglog { get; set; }
    }
}
