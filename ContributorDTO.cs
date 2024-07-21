using backendfepon.Models;

namespace backendfepon.DTOs.ContributorDTO
{
    public class ContributorDTO
    {

        public int id { get; set; }
        public string date { get; set; }
        public int state_id { get; set; }
        public string name { get; set; }
        public string faculty { get; set; }
        public string career { get; set; }
        public string email { get; set; }
        public string plan { get; set; }
        public decimal price { get; set; }
        public string academicPeriod { get; set; }
    }
}

