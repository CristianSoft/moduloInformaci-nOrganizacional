namespace backendfepon.DTOs.ContributionPlanDTOs
{
    public class ContributionPlanDTO
    {
        public int id { get; set; }
         public int state_id { get; set; }
        public string academicPeriod { get; set; }
        public string planName { get; set; }
        public string benefits { get; set; }
        public decimal price { get; set; }
    }
}
