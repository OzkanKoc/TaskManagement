using ProjectTaskManagement.Models.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.Mappings
{
    public class CustomerMapping : EntityTypeConfiguration<Customer>
    {
        public CustomerMapping()
        {
            Property(c => c.Address)
                .HasColumnType("varchar")
                .HasMaxLength(2000)
                .IsRequired();
            Property(c => c.CompanyName)
                .IsRequired()
                .HasColumnType("varchar")
                .HasMaxLength(100);
            Property(c => c.Email).IsRequired();
            Property(c => c.Phone).IsRequired();
        }
    }
}