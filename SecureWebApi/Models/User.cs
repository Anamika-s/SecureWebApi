using System.ComponentModel.DataAnnotations.Schema;

namespace SecureWebApi.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }
    }
}
