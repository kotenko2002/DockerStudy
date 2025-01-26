using API_2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public PingController(
            HttpClient httpClient,
            IConfiguration configuration,
            AppDbContext context)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("RevicePing")]
        public async Task<IActionResult> RevicePing()
        {
            Console.WriteLine("Ping received.");

            await _context.Pings.AddAsync(new Ping()
            {
                SenderName = ApiType.Api1,
                ReciverName = ApiType.Api2,
            });
            await _context.SaveChangesAsync();

            return Ok("Pong");
        }

        [HttpPost("SendPing")]
        public async Task<IActionResult> SendPing()
        {
            try
            {
                var targetUrl = $"http://{_configuration["AnotherApiHost"]}/Ping/RevicePing";
                var response = await _httpClient.GetAsync(targetUrl);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Ping sent successfully.");
                    var responseMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response: {responseMessage}");
                    return Ok("Ping sent successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to send ping. Status code: {response.StatusCode}");
                    return StatusCode((int)response.StatusCode, "Failed to send ping.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending ping: {ex.Message}");
                return StatusCode(500, "Error sending ping.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPings()
        {
            Console.WriteLine("Ping received.");

            var pings = await _context.Pings
                .Where(p => p.ReciverName == ApiType.Api2)
                .ToListAsync();

            return Ok(pings);
        }
    }
}
