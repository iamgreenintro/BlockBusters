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

        public IEnumerable<Genre> GetAllGenresForVideo(int videoId)
        {
            List<Genre> genres = new List<Genre>();

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using(SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    /*
                     The [dbo].[video_genres] table is *many to many* and we are checking for matches on the video.id and genre.id
                     Query steps:
                        1): Select all id and genre from genres table.
                        2): Use INNER JOIN to find all matches on video_genres table with genre.id and video_genre.genre_id.
                        3): From the collection of step 2 we only select the ones where the video_genre.video_id matches the passed video.id argument.

                        Note: The @videoId receives the value of videoId (passed as argument to this function) by the following line:
                        command.Parameters.AddWithValue("@videoId", videoId);
                     */
                    string query = "SELECT g.[id], g.[genre] FROM [dbo].[genres] g INNER JOIN [dbo].[video_genres] vg ON g.[id] = vg.[genre_id] WHERE vg.[video_id] = @videoId";

                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
                        {
                            // Replace the @videoId in the query with the videoId value.
                            command.Parameters.AddWithValue("@videoId", videoId);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Reads record by record as long as there are rows left
                                while (reader.Read())
                                {
                                    // Create and add/push the new Genre into our list with the assigned values.
                                    genres.Add(new Genre()
                                    {
                                        Id = (int)reader["id"], // Our record/row's "id" column value.
                                        Name = (string)reader["genre"], // Our record/row's "genre" column value.
                                    });
                                }
                            }
                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            // Rollback: Attempt to undo everything since the transaction started.
                            Console.Error.WriteLine("Attempting to rollback because an exception occured during the transaction phase.");
                            transaction.Rollback();
                        }
                        catch (Exception rollbackEx)
                        {
                            // Throw rollback exception
                            Console.Error.WriteLine("Rollback failed!");
                            throw new Exception($"{rollbackEx.GetType()} {rollbackEx.Message}");
                        }

                        // Throw general exception
                        throw new Exception($"{ex.GetType()} {ex.Message}");
                    }
                }
            }

            return genres;
        }
    }

}