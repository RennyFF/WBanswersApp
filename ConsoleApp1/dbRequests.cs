using System.Data.SQLite;

namespace ConsoleApp1
{
    class dbRequests
    {
        public readonly string _connectionStringUsers = "userdata.db";
        public readonly string _connectionStringAnswers = "answers.db";
        public readonly string _UsersName = "Users";
        public readonly string _AnswersName = "Answers";
        public bool CreateDBUsers()
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionStringUsers}"))
            {
                connection.Open();
                SQLiteCommand command = new();
                command.Connection = connection;
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {_UsersName}(Id INTEGER NOT NULL PRIMARY KEY UNIQUE, UserName TEXT, " +
                                      "TokenContent TEXT, TokenFeedBack TEXT, Preset TEXT)";
                try
                {
                    int _ = command.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }

        public List<UsersStructure> GetDBUsers()
        {
            List<UsersStructure> result = new();

            using (var connection = new SQLiteConnection($"Data Source={_connectionStringUsers}"))
            {
                connection.Open();

                SQLiteCommand command = new($"SELECT * FROM {_UsersName};", connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UsersStructure user = new();

                        user.Id = Convert.ToInt32(reader["Id"]);
                        user.UserName = reader["UserName"].ToString();
                        user.TokenContent = reader["TokenContent"].ToString();
                        user.TokenFeedBack = reader["TokenFeedback"].ToString();
                        user.Preset = reader["Preset"].ToString();

                        result.Add(user);
                    }
                }
            }

            return result;
        }
        public bool AddDBUsers(UsersStructure user)
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionStringUsers}"))
            {
                connection.Open();

                SQLiteCommand command = new();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO {_UsersName} (Id, UserName, TokenContent, TokenFeedback, Preset) " +
                                      $"VALUES (@Id, @UserName, @TokenContent, @TokenFeedback, @Preset)";
                command.Parameters.AddWithValue("@Id", GetLastNewId(_connectionStringUsers, _UsersName));
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@TokenContent", user.TokenContent);
                command.Parameters.AddWithValue("@TokenFeedback", user.TokenFeedBack);
                command.Parameters.AddWithValue("@Preset", user.Preset);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }

        public bool UpdateDBUser(UsersStructure user)
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionStringUsers}"))
            {
                connection.Open();

                SQLiteCommand command = new(connection);
                command.CommandText = $"UPDATE Users SET UserName = @UserName, TokenContent = @TokenContent, TokenFeedback = @TokenFeedback, Preset = @Preset WHERE Id = @Id";

                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@TokenContent", user.TokenContent);
                command.Parameters.AddWithValue("@TokenFeedback", user.TokenFeedBack);
                command.Parameters.AddWithValue("@Preset", user.Preset);
                command.Parameters.AddWithValue("@Id", user.Id);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }

        public bool CreateDBAnsw()
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionStringAnswers}"))
            {
                connection.Open();
                SQLiteCommand command = new();
                command.Connection = connection;
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {_AnswersName}(Id INTEGER NOT NULL PRIMARY KEY UNIQUE, Title TEXT, " +
                                      "Priority INTEGER, IsUsed INTEGER, Pattern TEXT, isRating INTEGER, TargetRating TEXT, Text TEXT, UserId INTEGER, FOREIGN KEY (UserId) REFERENCES Users(Id))";
                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }
        public List<AnswersStructure> GetDBAnsw()
        {
            List<AnswersStructure> result = new();

            using (var connection = new SQLiteConnection($"Data Source={_connectionStringAnswers}"))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {_AnswersName};", connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AnswersStructure answer = new();

                        answer.Id = Convert.ToInt32(reader["Id"]);
                        answer.Title = reader["Title"].ToString();
                        answer.Priority = Convert.ToInt32(reader["Priority"]);
                        answer.IsUsed = Convert.ToBoolean(reader["IsUsed"]);
                        answer.Pattern = reader["Pattern"].ToString();
                        answer.IsRating = Convert.ToBoolean(reader["IsRating"]);
                        answer.TargetRating = reader["TargetRating"].ToString();
                        answer.Text = reader["Text"].ToString();

                        result.Add(answer);
                    }
                }
            }

            return result;
        }
        public bool AddDBAnsw(AnswersStructure answer)
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionStringAnswers}"))
            {
                connection.Open();

                SQLiteCommand command = new();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO Answers (Id, Title, Priority, IsUsed, Pattern, IsRating, TargetRating, Text, UserId) " +
                                      $"VALUES (@Id, @Title, @Priority, @IsUsed, @Pattern, @IsRating, @TargetRating, @Text, @UserId)";
                command.Parameters.AddWithValue("@Id", GetLastNewId(_connectionStringAnswers, _AnswersName));
                command.Parameters.AddWithValue("@Title", answer.Title);
                command.Parameters.AddWithValue("@Priority", answer.Priority);
                command.Parameters.AddWithValue("@IsUsed", answer.IsUsed ? 1 : 0);
                command.Parameters.AddWithValue("@Pattern", answer.Pattern != null ? answer.Pattern: DBNull.Value);
                command.Parameters.AddWithValue("@IsRating", answer.IsRating ? 1 : 0);
                command.Parameters.AddWithValue("@TargetRating", answer.TargetRating);
                command.Parameters.AddWithValue("@Text", answer.Text);
                command.Parameters.AddWithValue("@UserId", answer.UserId);
                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }

        public bool UpdateDBAnsw(AnswersStructure answer)
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionStringAnswers}"))
            {
                connection.Open();

                SQLiteCommand command = new(connection);
                command.CommandText = $"UPDATE Answers SET Title = @Title, Priority = @Priority, IsUsed = @IsUsed, Pattern = @Pattern, IsRating = @IsRating, TargetRating = @TargetRating, Text = @Text WHERE Id = @Id";

                command.Parameters.AddWithValue("@Title", answer.Title);
                command.Parameters.AddWithValue("@Priority", answer.Priority);
                command.Parameters.AddWithValue("@IsUsed", answer.IsUsed ? 1 : 0);
                command.Parameters.AddWithValue("@Pattern", answer.Pattern);
                command.Parameters.AddWithValue("@IsRating", answer.IsRating ? 1 : 0);
                command.Parameters.AddWithValue("@TargetRating", answer.TargetRating);
                command.Parameters.AddWithValue("@Text", answer.Text);
                command.Parameters.AddWithValue("@Id", answer.Id);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }

        public bool DeleteRowDB(string _connectionString, string _dbName, int id)
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionString}"))
            {
                connection.Open();

                SQLiteCommand command = new(connection);
                command.CommandText = $"DELETE FROM {_dbName} WHERE Id = @Id";

                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }

        public int GetLastNewId(string _dbConnection, string _dbName)
        {
            using (var connection = new SQLiteConnection($"Data Source={_dbConnection}"))
            {
                connection.Open();

                SQLiteCommand command = new();
                command.Connection = connection;
                command.CommandText = $"SELECT MAX(Id) FROM {_dbName};";
                try
                {
                    object _type = command.ExecuteScalar();
                    int _lastId = int.TryParse(command.ExecuteScalar().ToString(), out _) ? Convert.ToInt32(_type) : 0;
                    _lastId += 1;
                    return _lastId;
                }
                catch (SQLiteException e)
                {
                    return -1;
                }
            }

        }
    }
}
