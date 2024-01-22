using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Controllers
{
    [Route("api/urlshortener")]
    [ApiController]
    public class RedirectURLController: ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public RedirectURLController(ApplicationDBContext context)
        {
            _context = context;
        }


        [HttpGet("{code}")]
        public IActionResult RedirectToUrl([FromRoute] string code)
        {
            // Retrieve the original URL based on the provided code
            var shortenedUrl = _context.ShortenedUrls.FirstOrDefault(u => u.UniqueCode == code);

            if (shortenedUrl == null)
            {
                return NotFound();
            }
            return RedirectPermanent(shortenedUrl.LongUrl);

        }

        private string redirectroute(string url)
        {
            string result = string.Empty;
            using (var client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    result = response.Headers.Location.ToString();
                }
            }
            return result;
        }
        
    }
}
