using Microsoft.Playwright;

namespace tests_e2e.Pages
{
	public class HomePage 
	{
		private readonly IPage _page;
		private readonly ILocator _searchBox;
		private readonly ILocator _acceptCookiesButton;
		private readonly ILocator _continueButton;
		private readonly ILocator _addToCartButton;
		private readonly ILocator _continueShoppingButton;
		private readonly ILocator _basketButton;
		private readonly ILocator _loginButton;
		private readonly string baseUrl = "https://www.bol.com/be/nl/";
		
		public HomePage(IPage page)
		{
			_page = page;
			_searchBox = page.Locator("[data-test=\"search_input_trigger\"]");
			_acceptCookiesButton = page.GetByRole(AriaRole.Button, new() { Name = "Alles accepteren" });
			_continueButton = page.GetByRole(AriaRole.Button, new() { Name = "Doorgaan" });
			_addToCartButton = page.Locator("[data-test=\"default-buy-block\"]").GetByRole(AriaRole.Button, new() { Name = "In winkelwagen" });
			_continueShoppingButton = page.GetByTestId("continue-shopping");
			_basketButton = page.Locator("[data-test=\"basket-button\"]");
			_loginButton = page.GetByRole(AriaRole.Link, new() { Name = "Inloggen" });

		}
		
		public async Task GoToHomePage()
		{
			await _page.GotoAsync(baseUrl);
		}
		
		public async Task AcceptCookies()
		{
			await _acceptCookiesButton.ClickAsync();
		}
		
		public async Task Continue()
		{
			await _continueButton.ClickAsync();
		}
		
		public async Task PutItemsInCart(string searchTerm, string expectedItem)
		{
			await _searchBox.ClickAsync();
			await _searchBox.FillAsync(searchTerm);
			await _searchBox.PressAsync("Enter");
			await _page.GetByRole(AriaRole.Heading, new() { Name = expectedItem }).ClickAsync();
			await _addToCartButton.ClickAsync();
		}
		
		public async Task ContinueShopping()
		{
			await _continueShoppingButton.ClickAsync();
		}
		public async Task GoToCartPage()
		{
			await _basketButton.ClickAsync();
		}
		
		public async Task GoToLoginPage()
		{
			await _loginButton.ClickAsync();
		}
	}
}