using System.Diagnostics;
using Microsoft.Playwright;
using NUnit.Framework;

namespace tests_e2e;

public class Tests
{
	private string baseUrl = "https://www.bol.com/be/nl/";
	private IPlaywright _playwright;
	private IBrowser _browser;
	private IBrowserContext _context;
	private IPage _page;
	
	[SetUp]
	public async Task Setup()
	{
		_playwright = await Playwright.CreateAsync();
		_browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		{
			Headless = false
		});
		_context = await _browser.NewContextAsync();
		_page = await _context.NewPageAsync();
	}

	[Test]
	public async Task NavigatetoPage()
	{
		await _page.GotoAsync(baseUrl);
		await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
		Assert.That(_page.Url, Is.EqualTo(baseUrl));
	}
	
	// [TearDown]
	// 	public async Task TearDown()
	// 	{
	// 		await _context.CloseAsync();
	// 		await _browser.CloseAsync();
	// 		_playwright.Dispose();
	// 	}
}