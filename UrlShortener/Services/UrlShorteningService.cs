using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Services
{
    public class UrlShorteningService
    {
        public const int Noofcharinshortlink = 7;
        private const string Alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private readonly Random _random = new();
        private readonly ApplicationDBContext _dbcontext;
        public UrlShorteningService(ApplicationDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<string> GenerateUniqueCode()
        {
            //create an array with a lenth of 7 
            var codeChars = new char[Noofcharinshortlink];

            while (true)
            {
                for (int i = 0; i < Noofcharinshortlink; i++)
                {
                    int randomIndex = _random.Next(codeChars.Length - 1);

                    codeChars[i] = Alphabets[randomIndex];
                }

                var code = new string(codeChars);

                if (!await _dbcontext.ShortenedUrls.AnyAsync(s => s.UniqueCode == code))
                {
                    return code;
                }
            }
        }
    }
}
