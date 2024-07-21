namespace backendfepon.Models
{
    public class Semester
    {
        public int Semester_Id { get; set; }
        public string Semester_Name { get; set; }

        public ICollection<AdministrativeMember> administrativeMembers { get; set; }

    }
}
