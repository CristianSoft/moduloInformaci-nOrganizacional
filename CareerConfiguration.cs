using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class CareerConfiguration : IEntityTypeConfiguration<Career>
    {
        public void Configure(EntityTypeBuilder<Career> modelBuilder)
        {
            modelBuilder.ToTable("CAREER");
            modelBuilder.HasKey(p => p.Career_Id);
        }
    }
}
