using Microsoft.Data.SqlClient;
using Repository.Enums;
using Repository.Interfaces;
using Repository.Models;
using System.Data;

namespace SqlRepository_AdoNet
{
    public class SqlRepository : IRepository
    {
        private const string _connectionString = "Server=.\\SQLEXPRESS; Database=TestConfiguration; Trusted_Connection=True; Encrypt=False";
        private const string _getBrowserConfigsQuery = "SELECT * FROM BrowserConfigs b " +
            "JOIN SqlBrowserConfigSqlUser bu ON b.Id = bu.BrowserConfigsId " +
            "JOIN Users u ON u.Id = bu.UsersId " +
            "JOIN Tests t ON t.UserId = u.Id " +
            "JOIN TestSteps s ON s.TestId = t.Id ";
        private const string _insertBrowserConfigQuery = "INSERT INTO BrowserConfigs (BrowserName, BrowserVersion) OUTPUT INSERTED.Id VALUES (@BrowserName, @BrowserVersion)";
        private const string _insertUserQuery = "INSERT INTO Users (Role, Login, Password) OUTPUT INSERTED.Id VALUES (@Role, @Login, @Password)";
        private const string _insertBrowserUserQuery = "INSERT INTO SqlBrowserConfigSqlUser (BrowserConfigsId, UsersId) VALUES (@BrowserConfigsId, @UsersId)";
        private const string _insertTestQuery = "INSERT INTO Tests (Title, ExpectedResult, UserId) OUTPUT INSERTED.Id VALUES (@Title, @ExpectedResult, @UserId)";
        private const string _insertTestStepQuery = "INSERT INTO TestSteps (StepNumber, StepText, TestId) VALUES (@StepNumber, @StepText, @TestId)";

        public Config GetConfig()
        {
            var config = new Config
            {
                BrowserConfigs = new List<BrowserConfig>()
            };

            var resultsTable = new DataTable();

            using (var dataAdapter = new SqlDataAdapter(_getBrowserConfigsQuery, _connectionString))
            {
                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                resultsTable = dataSet.Tables[0];
            }

            int currentBrowserConfigId, currentUserId, currentTestId;

            for (int row = 0; row < resultsTable.Rows.Count;)
            {
                currentBrowserConfigId = (int)resultsTable.Rows[row][0];

                var browserConfig = new BrowserConfig
                {
                    BrowserName = Enum.Parse<Browser>((string)resultsTable.Rows[row][1]),
                    BrowserVersion = (string)resultsTable.Rows[row][2],
                    Users = new List<User>()
                };

                for (int i = currentBrowserConfigId; row < resultsTable.Rows.Count && i == (int)resultsTable.Rows[row][0];)
                {
                    currentUserId = (int)resultsTable.Rows[row][5];

                    var user = new User
                    {
                        Role = Enum.Parse<UserRole>((string)resultsTable.Rows[row][6]),
                        Login = (string)resultsTable.Rows[row][7],
                        Password = (string)resultsTable.Rows[row][8],
                        Tests = new List<Test>()
                    };

                    for (int j = currentUserId; row < resultsTable.Rows.Count && j == (int)resultsTable.Rows[row][5];)
                    {
                        currentTestId = (int)resultsTable.Rows[row][9];

                        var test = new Test
                        {
                            Title = (string)resultsTable.Rows[row][10],
                            ExpectedResult = (string)resultsTable.Rows[row][11],
                            Steps = new List<TestStep>()
                        };

                        for (int k = currentTestId; row < resultsTable.Rows.Count && k == (int)resultsTable.Rows[row][9]; row++)
                        {
                            var step = new TestStep
                            {
                                StepNumber = (int)resultsTable.Rows[row][14],
                                StepText = (string)resultsTable.Rows[row][15]
                            };

                            test.Steps.Add(step);
                        }

                        user.Tests.Add(test);
                    }

                    browserConfig.Users.Add(user);
                }

                config.BrowserConfigs.Add(browserConfig);
            }

            return config;
        }

        public void WriteConfig(Config config)
        {
            int currentBrowserConfigId, currentUserId, currentTestId;

            using (var connection = new SqlConnection(_connectionString))
            {
                var sqlCommand = new SqlCommand();

                config.BrowserConfigs.ForEach(cfg =>
                {
                    sqlCommand.Parameters.AddWithValue("@BrowserName", cfg.BrowserName.ToString());
                    sqlCommand.Parameters.AddWithValue("@BrowserVersion", cfg.BrowserVersion);
                    sqlCommand.CommandText = _insertBrowserConfigQuery;
                    currentBrowserConfigId = (int)RunInsertQuery(connection, sqlCommand).Rows[0][0];

                    cfg.Users.ForEach(user =>
                    {
                        sqlCommand.Parameters.AddWithValue("@Role", user.Role.ToString());
                        sqlCommand.Parameters.AddWithValue("@Login", user.Login);
                        sqlCommand.Parameters.AddWithValue("@Password", string.IsNullOrEmpty(user.Password) ? "password" : user.Password);
                        sqlCommand.CommandText = _insertUserQuery;
                        currentUserId = (int)RunInsertQuery(connection, sqlCommand).Rows[0][0];

                        sqlCommand.Parameters.AddWithValue("@BrowserConfigsId", currentBrowserConfigId);
                        sqlCommand.Parameters.AddWithValue("@UsersId", currentUserId);
                        sqlCommand.CommandText = _insertBrowserUserQuery;
                        RunInsertQuery(connection, sqlCommand);

                        user.Tests.ForEach(test =>
                        {
                            sqlCommand.Parameters.AddWithValue("@Title", test.Title);
                            sqlCommand.Parameters.AddWithValue(@"ExpectedResult", test.ExpectedResult);
                            sqlCommand.Parameters.AddWithValue(@"UserId", currentUserId);
                            sqlCommand.CommandText = _insertTestQuery;
                            currentTestId = (int)RunInsertQuery(connection, sqlCommand).Rows[0][0];

                            test.Steps.ForEach(step =>
                            {
                                sqlCommand.Parameters.AddWithValue("@StepNumber", step.StepNumber);
                                sqlCommand.Parameters.AddWithValue("@StepText", step.StepText);
                                sqlCommand.Parameters.AddWithValue("@TestId", currentTestId);
                                sqlCommand.CommandText = _insertTestStepQuery;
                                RunInsertQuery(connection, sqlCommand);
                            });
                        });
                    });
                });
            }
        }

        private DataTable RunInsertQuery(SqlConnection connection, SqlCommand command)
        {
            DataTable dataTable = new DataTable();

            using (command)
            {
                command.Connection = connection;
                connection.Open();
                var reader = command.ExecuteReader();
                dataTable.Load(reader);
                connection.Close();
                command.Parameters.Clear();
            };

            return dataTable;
        }
    }
}