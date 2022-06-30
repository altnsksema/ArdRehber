using System.ComponentModel.DataAnnotations.Schema;

namespace ArdRehber.Entities
{
    public class User
    {
       
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Email { get; set; }
           // public string Password { get; set; }
            public byte[] PasswordHash { get; set; } 
            public byte[] PasswordSalt { get; set; }

            public string RefreshToken { get; set; }=String.Empty;
            public DateTime? RefreshTokenEndDate { get; set; }

            public Nullable<int> UserTypeId { get; set; } = null;

           //[ForeignKey("UserTypeId")]
            public virtual UserType UserType { get; set; }



       // public virtual ICollection<Image> Images { get; set; }   //----sonra burada include işlemi yapılaacak controller içinde departmanda olduğu gibi 







    }
}
