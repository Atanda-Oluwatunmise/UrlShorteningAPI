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
        public IActionResult RedirectToUrl(string code)
        {
            // Retrieve the original URL based on the provided code
            var shortenedUrl = _context.ShortenedUrls.FirstOrDefault(u => u.UniqueCode == code);

            if (shortenedUrl == null)
            {
                return NotFound();
            }

            // Perform the redirection
            return RedirectPermanent(shortenedUrl.LongUrl);
        }
    }
}
