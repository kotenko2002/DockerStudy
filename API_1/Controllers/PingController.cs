using Microsoft.AspNetCore.Mvc;

namespace API_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public PingController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("RevicePing")]
        public IActionResult RevicePing()
        {
            Console.WriteLine("Ping received.");
            return Ok("Pong");
        }

        [HttpPost("SendPing")]
        public async Task<IActionResult> SendPing()
        {
            try
            {
                var targetUrl = "https://localhost:6001/Ping/RevicePing";
                var response = await _httpClient.PostAsync(targetUrl, null);

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
    }
}
