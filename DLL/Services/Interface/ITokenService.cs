using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIs;

namespace BLL.Services.Interface
{
    public interface ITokenService
    {
        public string GenerateToken(User user);

        public string? GetEmail();

        public string GetRole();
        void InvalidateToken(string token);
        bool IsTokenInvalidated(string token);
        void CleanupExpiredTokens();
        bool ValidateToken(string token);
    }
}
