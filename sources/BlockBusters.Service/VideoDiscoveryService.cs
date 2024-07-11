using BlockBusters.Service.Domain;
using BlockBusters.Shared;

namespace BlockBusters.Service
{
    public class VideoDiscoveryService
    {
        private readonly VideoRepository videoRepository;

        public VideoDiscoveryService(VideoRepository videoRepository)
        {
            this.videoRepository = videoRepository;
        }

        public IEnumerable<VideoDto> ShowAllVideos()
        {
            return this.videoRepository.GetAll().Select(video =>
            {
                return new VideoDto()
                {
                    Title = video.Title,
                    Description = video.Description,
                    Duration = video.Duration
                };
            });
        }
    }
}
