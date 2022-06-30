namespace ArdRehber.Entities
{
    public class UserType
    {
        public Nullable<int> UserTypeId { get; set; } = null;
        public string UserTypeName { get; set; }
        public virtual ICollection<User> Users { get; set; }
        
    }
}
