// Needed: grant impersonate on user::[web_user] to database-mapped MSI
[HttpGet]
[Route("impersonate")]
public async Task<IActionResult> Impersonate()
{
    return await RunQuery(async (conn) => {
        var revertCookie = await conn.ExecuteScalarAsync<Byte[]>("declare @c varbinary(8000); execute as user = 'web_user' with cookie into @c; select @c;");
        var qr = await conn.QuerySingleOrDefaultAsync<string>(_sql);
        await conn.ExecuteAsync("revert with cookie = @c", new { @c = revertCookie });
        return JsonDocument.Parse(qr).RootElement;
    });            
}

// Use an existing token, for example from
// az account get-access-token --resource "https://database.windows.net"
[HttpGet]
[Route("token")]
public async Task<IActionResult> Token()
{
    string token = string.Empty;
    string authHeader = HttpContext.Request.Headers?["Authorization"];
    if (!string.IsNullOrEmpty(authHeader)) {
        var authHeaderTokens = authHeader.Split(' ');
        if (authHeaderTokens.Count() == 2 && authHeaderTokens[0].Trim().ToLower() == "bearer") 
            token = authHeaderTokens[1].Trim();                                
    }
    return await RunQuery(async (conn) => {                
        var qr = await conn.QuerySingleOrDefaultAsync<string>(_sql);
        return JsonDocument.Parse(qr).RootElement;
    }, token);            
}