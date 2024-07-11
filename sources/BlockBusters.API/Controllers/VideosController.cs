using BlockBusters.Service;
using BlockBusters.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlockBusters.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly VideoDiscoveryService videoDiscoveryService;

        public VideosController(VideoDiscoveryService videoDiscoveryService)
        {
            this.videoDiscoveryService = videoDiscoveryService;
        }

        [HttpGet]
        public IEnumerable<VideoDto> Get()
        {
            return this.videoDiscoveryService.ShowAllVideos();
        }

        [HttpPost]
        public VideoDto Post([FromBody] VideoDto video)
        {
            return this.videoDiscoveryService.AddOneVideo(video);
        }
    }
}
