﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ElectronicShopper.DataAccess.Identity;

public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, int>
{
    public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null!) : base(context, describer)
    {
    }
}