using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;



namespace Jwap.DAL.Entities
{
    [Index(nameof(UserName))]
    public class User : IdentityUser
    {
        public string FName { set; get; }
        public string LName { set; get; }
        public string ProfilePicture { set; get; } = "Avatar";
        public string Caption { set; get; }
        public DateTime RegisterDate { set; get; } = DateTime.Now.Date;
        public string Password { set; get; }
        public bool RememberMe { set; get; } = false;
        public bool Online { set; get; } = false;
        public bool InCall { set; get; } = false;

        public virtual ICollection<Connections> UserConnectionIds { set; get; } = new HashSet<Connections>();
        public virtual ICollection<Message> Messages { set; get; } = new HashSet<Message>();
        public virtual ICollection<Friends> Friends { set; get; } = new HashSet<Friends>();
        public virtual ICollection<CallOffer> CallOffers { set; get; } = new HashSet<CallOffer>();
    }
}
