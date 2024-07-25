using Microsoft.Playwright;

namespace tests_e2e.Pages
{
	public class CartPage 
	{
		private readonly IPage _page;
		
		public CartPage(IPage page)
		{
			_page = page;
			
		}
		
		
	}
}