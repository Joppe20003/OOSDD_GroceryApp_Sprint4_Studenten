
namespace Grocery.Core.Models
{
    public partial class Client : Model
    {
        public Role Role { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public Client(int id, string name, string emailAddress, string password) : base(id, name)
        {
            EmailAddress = emailAddress;
            Password = password;
            Role = Role.None;
        }
    }
}
