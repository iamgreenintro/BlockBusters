using BlockBusters.Service.Domain;
using BlockBusters.Shared;
using Microsoft.IdentityModel.Tokens;

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
                if (video.Genres == null)
                {
                    Console.WriteLine($"Video {video.Title} has null Genres property.");
                }
                else
                {
                    Console.WriteLine($"Video {video.Title} has {video.Genres.Count()} genres.");
                }
                return new VideoDto()
                {
                    Title = video.Title,
                    Description = video.Description,
                    Duration = video.Duration,
                    VideoThumbUrl = video.ImageUrl,
                    //Genres = video.Genres.Select(genreName => new GenreDto { Genre = genreName }) // Is this the correct way?
                };
            });
        }
    }
}
