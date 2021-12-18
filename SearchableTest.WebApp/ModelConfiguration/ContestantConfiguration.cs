using SearchableTest.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace SearchableTest.WebApp.ModelConfiguration
{
    public class ContestantConfiguration : EntityTypeConfiguration<Contestant>
    {
        public ContestantConfiguration()
        {
            Property(p => p.Firstname).HasMaxLength(50);
            Property(p => p.Lastname).HasMaxLength(50);
            Property(p => p.Gender).HasMaxLength(10);
            Property(p => p.ImgUrl).HasMaxLength(1000);
            Property(p => p.Address).HasMaxLength(100);
        }
    }
}