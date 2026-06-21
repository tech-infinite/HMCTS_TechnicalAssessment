using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMCTS_LoginAutomationTests.PageObjects
{
    public class SecureAreaPage
    {
        private readonly IPage _page;
        private readonly string _baseUrl;

        public SecureAreaPage(IPage page, string baseUrl)
        {
            _page = page;
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public async Task<bool> IsLoadedAsync()
        {
            // Check URL contains /secure
            if (!_page.Url.Contains("/secure", StringComparison.OrdinalIgnoreCase))
                return false;

            // Check the heading text
            return await _page.IsVisibleAsync("h2:has-text('Secure Area')");
        }

        public async Task LogoutAsync()
        {
            await _page.ClickAsync("a.button[href='/logout']"); // tweak selector if needed
        }

       
    }
}
