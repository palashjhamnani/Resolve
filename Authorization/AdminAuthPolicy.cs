using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resolve.Authorization
{
    public class AdminAuthPolicy
    {

        public static string Name => "Admin";

        public static void Build(AuthorizationPolicyBuilder builder) =>
            builder.RequireClaim("groups", "773d56cf-4ede-494e-8823-1956116230f1");        

    }
}
