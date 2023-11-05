using CryptoPulse.Models;
using Microsoft.AspNetCore.Identity;

namespace CryptoPulse.Areas.Identity.Data
{
    public class IdentityUserUpdate : IdentityUser
    {
        // Other properties of IdentityUser

        // Navigation property for the reverse relationship
        public virtual ICollection<Coin> Coins { get; set; }
    }
}
