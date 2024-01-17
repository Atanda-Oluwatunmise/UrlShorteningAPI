using Microsoft.AspNetCore.Mvc;
using UrlShortener.Entites;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [Route("api/urlshortener")]
    [ApiController]
    public class ShortenUrlController: ControllerBase
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ApplicationDBContext _dbContext;
        private readonly UrlShorteningService _urlShorteningService;
        public ShortenUrlController(IHttpContextAccessor httpContext, ApplicationDBContext dbContext, UrlShorteningService urlShorteningService)
        {
            _httpContext = httpContext;
            _dbContext = dbContext;
            _urlShorteningService = urlShorteningService;
        }

        [HttpPost("shorten")]
        public async Task<IResult> ShortenUrl(ShortenUrlRequest shortenUrlRequest)
        {
            //confirm if url passed is a proper url
            if(!Uri.TryCreate(shortenUrlRequest.Url, UriKind.Absolute, out _))
            {
                return Results.BadRequest("The specified Url is invalid");
            }

            var code = await _urlShorteningService.GenerateUniqueCode();
            var shortenedUrl = new ShortenedUrl
            {
                Id = Guid.NewGuid(),
                LongUrl = shortenUrlRequest.Url,
                ShortUrl = $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}/api/{code}",
                UniqueCode = code,
                DateCreated = DateTime.Now
            };

            _dbContext.ShortenedUrls.Add(shortenedUrl);
            await _dbContext.SaveChangesAsync();

            return Results.Ok(shortenedUrl.ShortUrl);
        }


    }
}
