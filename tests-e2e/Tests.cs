using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace tests_e2e;

public class Tests : PageTest
{
	private IBrowser _browser;
	private IBrowserContext _context;
	private IPage _page;
	
	private readonly string _email = "pxl3account@protonmail.com";
	private readonly string _password = "h.Sj5u3758yMXYh";
	
	[SetUp]
	public async Task Setup()
	{
		_browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		{
			Headless = false
		});
		_context = await _browser.NewContextAsync();
		_page = await _context.NewPageAsync();
	}

	[Test]
	public async Task CheckIfCartContainsCorrectItem()
	{
		await _page.GotoAsync("https://www.bol.com/be/nl/");
		await _page.GetByRole(AriaRole.Button, new() { Name = "Alles accepteren" }).ClickAsync();
		await _page.GetByRole(AriaRole.Button, new() { Name = "Doorgaan" }).ClickAsync();
		await _page.Locator("[data-test=\"search_input_trigger\"]").ClickAsync();
		await _page.Locator("[data-test=\"search_input_trigger\"]").FillAsync("maxxgarden stokparasol zwart");
		await _page.Locator("[data-test=\"search_input_trigger\"]").PressAsync("Enter");
		await _page.GetByRole(AriaRole.Heading, new() { Name = "MaxxGarden Stokparasol - tuin en balkon parasol - opdraaisysteem - 300 cm - Zwart", Exact = true }).ClickAsync();
		await _page.Locator("[data-test=\"default-buy-block\"]").GetByRole(AriaRole.Button, new() { Name = "In winkelwagen" }).ClickAsync();
		await _page.GetByTestId("continue-shopping").ClickAsync();
		await _page.Locator("[data-test=\"basket-button\"]").ClickAsync();
		await Expect(_page.GetByRole(AriaRole.Link, new() { Name = "MaxxGarden Stokparasol - tuin" })).ToBeVisibleAsync();
	}
	
	[TestCase("apollo systeemhalter zwart", "Apollo systeemhalters, set")]
	[TestCase("maxxgarden stokparasol zwart", "MaxxGarden Stokparasol - tuin en balkon parasol - opdraaisysteem - 300 cm - Zwart")]
	public async Task CheckIfCartContainsCorrectItem(string searchTerm, string expectedItem)
	{
		await _page.GotoAsync("https://www.bol.com/be/nl/");
		await _page.GetByRole(AriaRole.Button, new() { Name = "Alles accepteren" }).ClickAsync();
		await _page.GetByRole(AriaRole.Button, new() { Name = "Doorgaan" }).ClickAsync();
		await PutItemsInCart(searchTerm, expectedItem);
		await Expect(_page.GetByRole(AriaRole.Link, new(){ Name = expectedItem })).ToBeVisibleAsync();
	}
	
	private async Task Login()
	{
		await _page.GotoAsync("https://www.bol.com/be/nl/");
		await _page.GetByRole(AriaRole.Button, new() { Name = "Alles accepteren" }).ClickAsync();
		await _page.GetByRole(AriaRole.Button, new() { Name = "Doorgaan" }).ClickAsync();
		await _page.GetByRole(AriaRole.Link, new() { Name = "Inloggen" }).ClickAsync();
		await _page.GetByLabel("E-mailadres").ClickAsync();
		await _page.GetByLabel("E-mailadres").FillAsync(_email);
		await _page.GetByLabel("Wachtwoord").ClickAsync();
		await _page.GetByLabel("Wachtwoord").FillAsync(_password);
		await _page.GetByRole(AriaRole.Button, new() { Name = "Inloggen" }).ClickAsync();
	}
	
	private async Task PutItemsInCart(string searchTerm, string expectedItem)
	{
		await _page.Locator("[data-test=\"search_input_trigger\"]").ClickAsync();
		await _page.Locator("[data-test=\"search_input_trigger\"]").FillAsync(searchTerm);
		await _page.Locator("[data-test=\"search_input_trigger\"]").PressAsync("Enter");
		await _page.GetByRole(AriaRole.Heading, new() { Name = expectedItem }).ClickAsync();
		await _page.Locator("[data-test=\"default-buy-block\"]").GetByRole(AriaRole.Button, new() { Name = "In winkelwagen" }).ClickAsync();
		await _page.GetByTestId("continue-shopping").ClickAsync();
		await _page.Locator("[data-test=\"basket-button\"]").ClickAsync();
	}
	
	[TestCase("apollo systeemhalter zwart", "Apollo systeemhalters, set")]
	[TestCase("maxxgarden stokparasol zwart", "MaxxGarden Stokparasol - tuin en balkon parasol - opdraaisysteem - 300 cm - Zwart")]
	public async Task LoginAndCheckCart(string searchTerm, string expectedItem)
	{
		await Login();
		await PutItemsInCart(searchTerm, expectedItem);
	}

	[TearDown]
		public async Task TearDown()
		{
			await _context.CloseAsync();
			await _browser.CloseAsync();
			await _page.CloseAsync();
		}
	
}