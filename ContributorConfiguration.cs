using backendfepon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backendfepon.ModelConfigurations
{
    public class ContributorConfiguration : IEntityTypeConfiguration<Contributor>
    {
        public void Configure(EntityTypeBuilder<Contributor> modelBuilder)
        {
            modelBuilder.ToTable("CONTRIBUTOR");
            modelBuilder.HasKey(p => p.Contributor_Id);


            // Relationships
            modelBuilder.HasOne(cp => cp.ContributionPlan)
                .WithMany(c => c.Contributors)
                .HasForeignKey(c => c.Plan_Id);

            modelBuilder
               .HasOne(p => p.State)
               .WithMany(s => s.Contributors)
               .HasForeignKey(p => p.State_Id);

            modelBuilder
               .HasOne(p => p.Career)
               .WithMany(s => s.Contributors)
               .HasForeignKey(p => p.Career_Id);

            modelBuilder
               .HasOne(p => p.Faculty)
               .WithMany(s => s.Contributors)
               .HasForeignKey(p => p.Faculty_Id);



        }
    }
}
