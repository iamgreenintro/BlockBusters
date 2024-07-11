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

        // Instead of conflicting routes for different body types, we will just pass an array; even when we want to update only 1.
        //[HttpPost]
        //public VideoDto Post([FromBody] VideoDto video)
        //{
        //    return this.videoDiscoveryService.AddOneVideo(video);
        //}

        [HttpPost]
        public IEnumerable<VideoDto> PostMultiple([FromBody] IEnumerable<VideoDto> videos)
        {
            return this.videoDiscoveryService.AddMultipleVideos(videos);
        }
    }
}
