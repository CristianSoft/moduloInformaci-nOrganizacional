namespace backendfepon.Models
{
    public class Role
    {
        public int Role_Id { get; set; }
        public string Role_Name { get; set; }

        public ICollection<AdministrativeMember> Administrative_Members { get; set; }
    }
}
