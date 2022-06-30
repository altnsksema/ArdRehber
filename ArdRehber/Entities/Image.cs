using System.ComponentModel.DataAnnotations.Schema;

namespace ArdRehber.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] ImageBase64 { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

       // public byte[] ProfilePhoto { get; set; }

        [NotMapped]
        public string Profile
        {
            get
            {
                return System.Text.Encoding.Default.GetString(ImageBase64);
            }
            set { }
        }
    }
}
