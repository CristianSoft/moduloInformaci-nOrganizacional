namespace backendfepon.Models
{
    public class Career
    {
        public int Career_Id { get; set; }
        public string Career_Name { get; set; }

        public ICollection<AdministrativeMember> administrativeMembers { get; set; }
        public ICollection<Contributor> Contributors { get; set; }
    }
}
