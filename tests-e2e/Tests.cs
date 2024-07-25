using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using tests_e2e.Pages;

namespace tests_e2e;

public class Tests : PageTest
{
	private IBrowser _browser;
	private IBrowserContext _context;
	private IPage _page;
	private HomePage _homePage;
	private CartPage _cartPage;
	private LoginPage _loginPage;
	
	[SetUp]
	public async Task Setup()
	{
		_browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		{
			Headless = false
		});
		_context = await _browser.NewContextAsync();
		_page = await _context.NewPageAsync();
		_homePage = new HomePage(_page);
		_cartPage = new CartPage(_page);
		_loginPage = new LoginPage(_page);
	}
	// this test has become obsolete due to the parameterized test. Is still present for documentation purposes.
	// [Test]
	// public async Task CheckIfCartContainsCorrectItem()
	// {
	// 	_homePage.GoToHomePage();
	// 	_homePage.AcceptCookies();
	// 	_homePage.Continue();
	// 	await _page.Locator("[data-test=\"search_input_trigger\"]").ClickAsync();
	// 	await _page.Locator("[data-test=\"search_input_trigger\"]").FillAsync("maxxgarden stokparasol zwart");
	// 	await _page.Locator("[data-test=\"search_input_trigger\"]").PressAsync("Enter");
	// 	await _page.GetByRole(AriaRole.Heading, new() { Name = "MaxxGarden Stokparasol - tuin en balkon parasol - opdraaisysteem - 300 cm - Zwart", Exact = true }).ClickAsync();
	// 	await _page.Locator("[data-test=\"default-buy-block\"]").GetByRole(AriaRole.Button, new() { Name = "In winkelwagen" }).ClickAsync();
	// 	await _page.GetByTestId("continue-shopping").ClickAsync();
	// 	await _page.Locator("[data-test=\"basket-button\"]").ClickAsync();
	// 	await Expect(_page.GetByRole(AriaRole.Link, new() { Name = "MaxxGarden Stokparasol - tuin" })).ToBeVisibleAsync();
	// }
	
	[TestCase("apollo systeemhalter zwart", "Apollo systeemhalters, set")]
	[TestCase("maxxgarden stokparasol zwart", "MaxxGarden Stokparasol - tuin en balkon parasol - opdraaisysteem - 300 cm - Zwart")]
	public async Task CheckIfCartContainsCorrectItem(string searchTerm, string expectedItem)
	{
		await _homePage.GoToHomePage();
		await _homePage.AcceptCookies();
		await _homePage.Continue();
		await _homePage.PutItemsInCart(searchTerm, expectedItem);
		await _homePage.ContinueShopping();
		await _homePage.GoToCartPage();
		await Expect(_page.GetByRole(AriaRole.Link, new(){ Name = expectedItem })).ToBeVisibleAsync();
	}
	
	[TestCase("apollo systeemhalter zwart", "Apollo systeemhalters, set")]
	[TestCase("maxxgarden stokparasol zwart", "MaxxGarden Stokparasol - tuin en balkon parasol - opdraaisysteem - 300 cm - Zwart")]
	public async Task LoginAndCheckCart(string searchTerm, string expectedItem)
	{
		await Login();
		await _homePage.PutItemsInCart(searchTerm, expectedItem);
		await _homePage.ContinueShopping();
		await _homePage.GoToCartPage();
		await Expect(_page.GetByRole(AriaRole.Link, new(){ Name = expectedItem })).ToBeVisibleAsync();
	}
	
	[TestCase("apollo systeemhalter zwart", "Apollo systeemhalters, set")]
	public async Task CheckIfItemIsRemoved(string searchTerm, string expectedItem)
	{
		await Login();
		await _homePage.PutItemsInCart(searchTerm, expectedItem);
		await _homePage.ContinueShopping();
		await _homePage.GoToCartPage();
		await _cartPage.RemoveItem(expectedItem);
		await Expect(_page.GetByRole(AriaRole.Link, new() { Name = expectedItem })).ToBeHiddenAsync();
	}
	
	private async Task Login()
	{
		await _homePage.GoToHomePage();
		await _homePage.AcceptCookies();
		await _homePage.Continue();
		await _homePage.GoToLoginPage();
		await _loginPage.EnterEmail();
		await _loginPage.EnterPassword();
		await _loginPage.ClickLoginButton();
	}
	
	[TearDown]
		public async Task TearDown()
		{
			await _context.CloseAsync();
			await _browser.CloseAsync();
			await _page.CloseAsync();
		}
	
}