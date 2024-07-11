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
                    Genres = this.genreRepository.getAllGenres().Select(genre =>
                    {
                        //if(genre.Id == video.Id)
                        //{
                            return new GenreDto { Genre = genre.Name };
                        //}
                        //else
                        //{
                        //    return null;
                        //}
                    }) 
                    //Genres = video.Genres.Select(genreName => new GenreDto { Genre = genreName }) // Is this the correct way?
                };
            });
        }
    }
}
