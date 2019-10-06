using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Repules.Model
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {

        public ApplicationUserRole()
        {
        }
    }
}
