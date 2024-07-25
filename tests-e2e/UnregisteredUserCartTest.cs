using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace tests_e2e;

public class Tests : PageTest
{
	private IBrowser _browser;
	private IBrowserContext _context;
	private IPage _page;
	
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
	public async Task CheckIfCartContainsCorrectItems()
	{
		await _page.GotoAsync("https://www.bol.com/be/nl/");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Alles accepteren" }).ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Doorgaan" }).ClickAsync();
        await _page.Locator("[data-test=\"search_input_trigger\"]").ClickAsync();
        await _page.Locator("[data-test=\"search_input_trigger\"]").FillAsync("parasol");
        await _page.Locator("[data-test=\"search_input_trigger\"]").PressAsync("Enter");
        await _page.GetByRole(AriaRole.Heading, new() { Name = "MaxxGarden Stokparasol - tuin en balkon parasol - opdraaisysteem - 300 cm - Zwart", Exact = true }).ClickAsync();
        await _page.Locator("[data-test=\"default-buy-block\"]").GetByRole(AriaRole.Button, new() { Name = "In winkelwagen" }).ClickAsync();
        await _page.GetByTestId("continue-shopping").ClickAsync();
        await _page.Locator("[data-test=\"basket-button\"]").ClickAsync();
        await Expect(_page.GetByRole(AriaRole.Link, new() { Name = "MaxxGarden Stokparasol - tuin" })).ToBeVisibleAsync();
	}
	
	[TearDown]
		public async Task TearDown()
		{
			await _context.CloseAsync();
			await _browser.CloseAsync();
			await _page.CloseAsync();
		}
	
}