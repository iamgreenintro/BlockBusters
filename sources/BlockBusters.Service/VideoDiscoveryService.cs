using BlockBusters.Service.Domain;
using BlockBusters.Shared;

namespace BlockBusters.Service
{
    public class VideoDiscoveryService
    {
        private readonly VideoRepository videoRepository;
        private readonly GenreRepository genreRepository;

        public VideoDiscoveryService(VideoRepository videoRepository, GenreRepository genreRepository)
        {
            this.videoRepository = videoRepository;
            this.genreRepository = genreRepository;
        }

        public IEnumerable<VideoDto> ShowAllVideos()
        {
            return this.videoRepository.GetAll().Select(video =>
            {
                return new VideoDto()
                {
                    Title = video.Title,
                    Description = video.Description,
                    Duration = video.Duration,
                    VideoThumbUrl = video.ImageUrl,
                    Genres = this.genreRepository.getAllGenresForVideo(video.Id).Select(genre =>
                    {

                        return new GenreDto { Genre = genre.Name };

                    })
                };
            });
        }

        public VideoDto AddOneVideo(VideoDto video)
        {
            var createdVideo = this.videoRepository.CreateOne(video);
            VideoDto resultVideoDto = new VideoDto()
            {
                Title = createdVideo.Title,
                Description = createdVideo.Description,
                Duration = createdVideo.Duration,
                VideoThumbUrl = createdVideo.ImageUrl,
                Genres = this.genreRepository.getAllGenresForVideo(createdVideo.Id).Select(genre =>
                {
                    return new GenreDto { Genre = genre.Name };
                })
            };

            return resultVideoDto;
        }
    }
}
