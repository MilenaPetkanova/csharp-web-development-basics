namespace IRunesWebApp.Models
{
    using System.Collections.Generic;

    public class User : BaseEntity<string>
    {
        public User()
        {
            this.Albums = new HashSet<Album>();
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public ICollection<Album> Albums { get; set; }
    }
}