using Microsoft.EntityFrameworkCore.Update.Internal;

namespace backendfepon.Models
{
    public class AdministrativeMember
    {
        public int Administrative_Member_Id { get; set; }
        public int State_Id { get; set; }
        public int Faculty_Id { get; set; }
        public int Career_Id { get; set; }
        public int Role_Id { get; set; }
        public int Semester_Id { get; set; }

        public string Name { get; set; }
        public string Last_Name { get; set; }
        public DateTime Birth_Date { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public Faculty Faculty { get; set; }
        public Career Career { get; set; }
        public Semester Semester { get; set; }

        public Responsible Responsible { get; set; }

        public Role Role { get; set; }
        public State State { get; set; }

    }
}
