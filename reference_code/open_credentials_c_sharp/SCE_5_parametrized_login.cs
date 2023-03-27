[HttpPost]
public async Task<ActionResult> Login(LoginViewModel model)
{
    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
    if (result.Succeeded)
    {
    return RedirectToAction("Index");
    }
    else
    {
    return View();
    }
}