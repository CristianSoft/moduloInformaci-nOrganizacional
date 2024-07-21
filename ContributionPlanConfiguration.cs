using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace backendfepon.ModelConfigurations
{
    public class ContributionPlanConfiguration : IEntityTypeConfiguration<ContributionPlan>
    {
        public void Configure(EntityTypeBuilder<ContributionPlan> modelBuilder)
        {
            modelBuilder.ToTable("CONTRIBUTION_PLAN");
            modelBuilder.HasKey(p => p.Plan_Id);
            modelBuilder
               .HasOne(p => p.State)
               .WithMany(s => s.ContributionPlans)
               .HasForeignKey(p => p.State_Id);

            modelBuilder
               .HasOne(p => p.AcademicPeriod)
               .WithMany(s => s.ContributionPlans)
               .HasForeignKey(p => p.Academic_Period_Id);

            modelBuilder.HasMany(cp => cp.Contributors)
                .WithOne(c => c.ContributionPlan)
                .HasForeignKey(c => c.Plan_Id);


        }
    }
}
