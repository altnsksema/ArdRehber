using System.ComponentModel.DataAnnotations.Schema;

namespace ArdRehber.Entities
{
    public class TitleDepartment
    {
        public int Id { get; set; }

        public int TitleId { get; set; }

        [ForeignKey("TitleId")]
        public virtual Title Title { get; set; }

        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }




      

    }
}
