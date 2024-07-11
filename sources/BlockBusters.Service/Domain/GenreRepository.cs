using BlockBusters.Shared;
using Microsoft.Data.SqlClient;

namespace BlockBusters.Service.Domain
{
    public class GenreRepository
    {
        private readonly string connectionString;

        public GenreRepository(IBlockBustersConnection blockBustersConnection)
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

            connectionString = connectionBuilder.ToString();
        }

        public IEnumerable<Genre> getAllGenresForVideo(int videoId)
        {
            List<Genre> genres = new List<Genre>();

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using(SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    using (SqlCommand command = new SqlCommand(@"
                SELECT g.[id], g.[genre] 
                FROM [dbo].[genres] g
                INNER JOIN [dbo].[video_genres] vg ON g.[id] = vg.[genre_id]
                WHERE vg.[video_id] = @videoId", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@videoId", videoId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                genres.Add(new Genre()
                                {
                                    Id = (int)reader["id"],
                                    Name = (string)reader["genre"],
                                });
                            }
                        }
                    }

                    transaction.Commit();

                }
            }

            return genres;
        }
    }

}