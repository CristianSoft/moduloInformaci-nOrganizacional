namespace backendfepon.Models
{
    public class AcademicPeriod
    {
        public int Academic_Period_Id { get; set; }
        public string Academic_Period_Name { get; set; }

        // Navigation property
        public ICollection<ContributionPlan> ContributionPlans { get; set; }
    }
}
