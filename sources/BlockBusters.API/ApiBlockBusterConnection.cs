using BlockBusters.Shared;

namespace BlockBusters.API
{
    public class ApiBlockBusterConnection : IBlockBustersConnection
    {
        public string Server { get => ".\\" ; set => throw new NotImplementedException(); }
        public string Database { get => "BlockBusters" ; set => throw new NotImplementedException(); }
        public string Username { get => ""; set => throw new NotImplementedException(); }
        public string Password { get => "" ; set => throw new NotImplementedException(); }
    }
}