namespace SecureWebApi.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public virtual ICollection<User>? Users { get; set; }
    }
}

// Seeding data 
// you want to provide some data from your own without using any 
// inteface