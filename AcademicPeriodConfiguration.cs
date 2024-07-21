using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class AcademicPeriodConfiguration : IEntityTypeConfiguration<AcademicPeriod>
    {
        public void Configure(EntityTypeBuilder<AcademicPeriod> modelBuilder)
        {
            modelBuilder.ToTable("ACADEMIC_PERIOD");
            modelBuilder.HasKey(p => p.Academic_Period_Id);

        }
    }

}
