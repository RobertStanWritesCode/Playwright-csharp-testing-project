using System.Text.RegularExpressions;
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
		
		public async Task RemoveItem(string itemName)
		{
			var itemContainer = _page.GetByTestId("item-row").Filter(new() { HasText = itemName});
			var removeButton = itemContainer.GetByTestId("remove-item");
			await removeButton.ClickAsync();
		}
		
		
	}
}