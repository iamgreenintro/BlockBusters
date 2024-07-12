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

        // Returns all videos or an empty array.
        [HttpGet]
        public IEnumerable<VideoDto> Get()
        {
            return this.videoDiscoveryService.ShowAllVideos();
        }

        // Instead of conflicting routes for for a single video or multiple videos, we will just pass an array; even when we want to update only 1.
        // Returns the created video(s)
        [HttpPost]
        public IEnumerable<VideoDto> PostMultiple([FromBody] IEnumerable<VideoDto> videos)
        {
            return this.videoDiscoveryService.AddMultipleVideos(videos);
        }
    }
}
