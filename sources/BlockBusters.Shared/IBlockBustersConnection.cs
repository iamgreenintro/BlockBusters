namespace BlockBusters.Shared
{
    public interface IBlockBustersConnection {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Username { get; set ;}
        public string Password { get; set; }
    }
}
