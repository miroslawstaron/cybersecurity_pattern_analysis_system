public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
{
    Logger.SendLog("MyAuthorizationServerProvider/GrantResourceOwnerCredentials", context.UserName+" "+context.Password, null, LogTypes.Request);
    
    
    var identity = new ClaimsIdentity(context.Options.AuthenticationType);

    Users user = userRepo.FindUser(context.UserName, context.Password);

    if (user != null)
    {
        try
        {
            identity.AddClaim(new Claim("username", user.Username));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            

            if (user.Roles.RoleType == RoleTypes.SuperUser.ToString())
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, RoleTypes.SuperUser.ToString()));
            }
            else
                identity.AddClaim(new Claim(ClaimTypes.Role, RoleTypes.User.ToString()));

            context.Validated(identity);
            Logger.SendLog("MyAuthorizationServerProvider/GrantResourceOwnerCredentials", identity.Name,null,null);
        }
        catch (Exception ex)
        {
            Logger.SendLog("MyAuthorizationServerProvider/GrantResourceOwnerCredentials", ex, true, LogTypes.Response);
            context.SetError("invalid_grant", Messages.IdentityFail);
            return;
        }
        

    }
    else
    {

        Logger.SendLog( MethodBase.GetCurrentMethod().Name, Messages.UserNotFind,true,LogTypes.Response);
        context.SetError("invalid_grant", Messages.UserNotFind);
        return;
    }

}