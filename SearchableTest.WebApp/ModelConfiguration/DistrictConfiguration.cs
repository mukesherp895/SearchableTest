using SearchableTest.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace SearchableTest.WebApp.ModelConfiguration
{
    public class DistrictConfiguration : EntityTypeConfiguration<District>
    {
        public DistrictConfiguration()
        {
            Property(p => p.DistrictName).HasMaxLength(50);
        }
    }
}