using BlockBusters.Shared;
using Microsoft.Data.SqlClient;

namespace BlockBusters.Domain
{
    public class VideoRepository
    {
        private readonly string connectionString;

        public VideoRepository(IBlockBustersConnection blockBustersConnection)
        {
            SqlConnectionStringBuilder connectionBuilder = new SqlConnectionStringBuilder();
            connectionBuilder.TrustServerCertificate = true;
            connectionBuilder.Encrypt = null;
            connectionBuilder.DataSource = blockBustersConnection.Server;
            connectionBuilder.InitialCatalog = blockBustersConnection.Database;

            if (!string.IsNullOrWhiteSpace(blockBustersConnection.Username))
            {
                connectionBuilder.UserID = blockBustersConnection.Username;
                connectionBuilder.Password = blockBustersConnection.Password;
            }
            else
            {
                connectionBuilder.IntegratedSecurity = true;
            }

            this.connectionString = connectionBuilder.ToString();
        }

        public IEnumerable<Video> GetAll()
        {
            List<Video> videos = new List<Video>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    using (SqlCommand command = new SqlCommand("SELECT [id],[title],[duration],[image_url],[description] FROM [dbo].[videos]", connection, transaction))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                videos.Add(new Video()
                                {
                                    Id = (int)reader["id"],
                                    Title = (string)reader["title"],
                                    Duration = (int)reader["duration"],
                                    ImageUrl = (string)reader["image_url"],
                                    Description = (string)reader["description"]
                                });
                            }
                        }
                    }

                    transaction.Commit();
                }
            }

            return videos;
        }
    }
}