using System.ComponentModel.DataAnnotations;

namespace ArdRehber.Entities

{
    public class Department
    {   
        public int DepartmentId { get; set; }

        [StringLength(50)]
        public string DepartmentName { get; set; }

        //  public virtual ICollection<Person> Persons { get; set; }
        public virtual ICollection<TitleDepartment> TitleDepartments { get; set; }



    }
}
