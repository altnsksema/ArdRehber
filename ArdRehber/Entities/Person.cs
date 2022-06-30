using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArdRehber.Entities
{
    public class Person
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string SurName { get; set; }

        [StringLength(11)]
        public string PhoneNumber { get; set; }

        [StringLength(4)]
        public string InternalNumber { get; set; }

        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }


        public int? TitleId { get; set; }

        [ForeignKey("TitleId")]
        public virtual Title Title { get; set; }












    }
}
