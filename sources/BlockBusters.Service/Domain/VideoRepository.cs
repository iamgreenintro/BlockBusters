using BlockBusters.Shared;
using Microsoft.Data.SqlClient;

namespace BlockBusters.Service.Domain
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

            connectionString = connectionBuilder.ToString();
        }

        public IEnumerable<Video> GetAll()
        {
            List<Video> videos = new List<Video>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    string query = "SELECT [id],[title],[duration],[image_url],[description] FROM [dbo].[videos]";

                    try
                    {
                        using (SqlCommand command = new SqlCommand(query, connection, transaction))
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

            return videos;
        }


        // Previously we had a method that created 1 single video at a time and we called that method in a loop.
        // The issue with that was we were opening the connection to the database and querying in a loop.
        // This requires the videos to be passed as an array to the body but it can update one or more, which is fine for now.
        public IEnumerable<Video> CreateMultiple(IEnumerable<VideoDto> multipleVideosData)
        {
            List<Video> videos = new List<Video>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    string queryInsertVideos = "INSERT INTO [dbo].[videos] ([title], [duration], [image_url], [description]) VALUES (@Title, @Duration, @ImageUrl, @Description); SELECT SCOPE_IDENTITY();";

                    try
                    {
                        foreach (var videoData in multipleVideosData)
                        {
                            using (SqlCommand command = new SqlCommand(query, connection, transaction))
                            using (SqlCommand command = new SqlCommand(queryInsertVideos, connection, transaction))
                            {
                                // Replace query placeholders with the values of our videoData properties.
                                command.Parameters.AddWithValue("@Title", videoData.Title);
                                command.Parameters.AddWithValue("@Duration", videoData.Duration);
                                command.Parameters.AddWithValue("@ImageUrl", videoData.VideoThumbUrl);
                                command.Parameters.AddWithValue("@Description", videoData.Description);

                                // For casting purposes
                                int videoId;

                                // Receives the ID based on SELECT SCOPE_IDENTITY();  ExecuteScalar() always returns the first record it finds by the given command query
                                object result = command.ExecuteScalar();
                                
                                // If our result got populated and we can cast it to an int (expects an integer for the ID), have the value assigned to videoId.
                                if (result != null && int.TryParse(result.ToString(), out videoId))
                                {
                                    videos.Add(new Video()
                                    {
                                        Id = videoId,
                                        Title = videoData.Title,
                                        Duration = videoData.Duration,
                                        ImageUrl = videoData.VideoThumbUrl,
                                        Description = videoData.Description
                                    });
                                }
                                else
                                {
                                    throw new Exception("Unable to get the videoId of videos we inserted!");
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

            return videos;
        }
    }

}
