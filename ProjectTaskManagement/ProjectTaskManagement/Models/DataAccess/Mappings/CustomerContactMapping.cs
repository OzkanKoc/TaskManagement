using ProjectTaskManagement.Models.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.Mappings
{
    public class CustomerContactMapping : EntityTypeConfiguration<CustomerContact>
    {
        public CustomerContactMapping()
        {
            Property(c => c.ContactName)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);
            Property(c => c.ContactTitle)
                .HasColumnType("varchar")
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}