using Microsoft.Playwright;

namespace tests_e2e.Pages
{
	public class LoginPage
	{
		private readonly IPage _page;
		private readonly ILocator _emailLocator;
		private readonly ILocator _passwordLocator;
		private readonly ILocator _loginButton;
		private readonly string _email = "pxl3account@protonmail.com";
		private readonly string _password = "h.Sj5u3758yMXYh";		
		public LoginPage(IPage page)
		{
			_page = page;
			_emailLocator = page.GetByLabel("E-mailadres");
			_passwordLocator = page.GetByLabel("Wachtwoord");
			_loginButton = page.GetByRole(AriaRole.Button, new() { Name = "Inloggen" });
		}
		
		public async Task EnterEmail()
		{
			await _emailLocator.ClickAsync();
			await _emailLocator.FillAsync(_email);
		}
		
		public async Task EnterPassword()
		{
			await _passwordLocator.ClickAsync();
			await _passwordLocator.FillAsync(_password);
		}
		
		public async Task ClickLoginButton()
		{
			await _loginButton.ClickAsync();
		}
	}
}