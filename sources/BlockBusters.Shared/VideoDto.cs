namespace BlockBusters.Shared
{
    public class VideoDto
    {
        public string Title { get ; set ;}

        public string VideoThumbUrl { get ; set;}

        public int Duration { get; set; }

        public string Description { get; set; }

        public IEnumerable<GenreDto> Genres { get ;set ;}
    }
}
