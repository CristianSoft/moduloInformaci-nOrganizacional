namespace backendfepon.Models
{
    public class ContributionPlan
    {
        public int Plan_Id { get; set; }
        public int Academic_Period_Id { get; set; }
        public int State_Id { get; set; }
        public string Name { get; set; }
        public string Benefits { get; set; }
        public decimal Economic_Value { get; set; }

        public State State { get; set; }
        public AcademicPeriod AcademicPeriod { get; set; }
        public ICollection<Contributor> Contributors { get; set; }
    }
}
