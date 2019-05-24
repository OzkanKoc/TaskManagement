using ProjectTaskManagement.Models.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.Mappings
{
    public class CustomerDemandMapping : EntityTypeConfiguration<CustomerDemand>
    {
        public CustomerDemandMapping()
        {
            Property(c => c.Demand)
                .IsRequired()
                .HasMaxLength(300)
                .HasColumnType("varchar");
            Property(c => c.DemandType).IsRequired();
        }
    }
}