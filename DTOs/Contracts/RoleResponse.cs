﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Contracts
{
    public class RoleResponse
    {
        public string Role;

        public RoleResponse(string role)
        {
            Role = role;
        }

        public RoleResponse() { }
    }
}
