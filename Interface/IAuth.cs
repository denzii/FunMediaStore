using System.Security.Claims;
using FunMediaStore.Model;

namespace FunMediaStore.Interface
{
    public interface IAuth
    {
        public (byte[] passwordSalt, byte[] passwordHash) CreatePasswordHash(string password, byte[]? salt = null);
        public string GenerateToken(User user);
        public User GetClaimedUser(IEnumerable<Claim> claims);
        public bool isAuthentic(User user, byte[] passwordHash);
    }
}
