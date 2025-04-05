using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Contracts
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; } 

        public LoginResponse(string token, string email, string firstName, string lastName, string role)
        {
            Token = token;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
        }
    }
}


