using MySql.Data.MySqlClient;
using stepik.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stepik.Services
{
    public class UsersService
    {
        /// <summary>
        /// Получение пользователя из таблицы users
        /// </summary>
        /// <param name="fullName">Полное имя пользователя</param>
        /// <returns>User</returns>
        public static User? Get(string fullName)
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();
            var query = @"SELECT * FROM users
                   WHERE full_name = @FullName AND is_active = 1;";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@FullName", fullName);
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? new User
                {
                    FullName = reader.GetString("full_name"),
                    Details = reader.IsDBNull("details") ? null : reader.GetString("details"),
                    JoinDate = reader.GetDateTime("join_date"),
                    Avatar = reader.IsDBNull("avatar") ? null : reader.GetString("avatar"),
                    IsActive = reader.GetBoolean("is_active"),
                    Knowledge = reader.GetInt32("knowledge"),
                    Reputation = reader.GetInt32("reputation"),
                    FollowersCount = reader.GetInt32("followers_count")
                }
                : null;
        }

        public static bool Add(User user)
        {
            try
            {
                using (var connection = new MySqlConnection(Constant.ConnectionString))
                {
                    connection.Open();
                    string sqlQuery = @"INSERT INTO users (full_name, details, join_date, avatar, is_active)
										VALUES (@fullName, @details, @joinDate, @avatar, @isActive);";

                    using (var command = new MySqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@fullName", user.FullName);
                        command.Parameters.AddWithValue("@details", user.Details);
                        command.Parameters.AddWithValue("@joinDate", user.JoinDate);
                        command.Parameters.AddWithValue("@avatar", user.Avatar);
                        command.Parameters.AddWithValue("@isActive", user.IsActive);

                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int GetTotalCount()
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();

            string sqlQuery = "SELECT COUNT(id) FROM users";

            using var command = new MySqlCommand(sqlQuery, connection);
            object totalCount = command.ExecuteScalar();

            return Convert.ToInt32(totalCount);
        }

        /// <summary>
        /// Форматирование показателей пользователя
        /// </summary>
        /// <param name="number">Число для форматирования</param>
        /// <returns>Отформатированное число</returns>
        public static string FormatUserMetrics(int number)
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();

            string functionName = "format_number";

            using var command = new MySqlCommand(functionName, connection);
            command.CommandType = CommandType.StoredProcedure;
            var numParam = new MySqlParameter("num", number)
            {
                Direction = ParameterDirection.Input,
            };
            var returnValueParam = new MySqlParameter()
            {
                Direction = ParameterDirection.ReturnValue
            };
            command.Parameters.Add(numParam);
            command.Parameters.Add(returnValueParam);

            command.ExecuteNonQuery();
            return returnValueParam.Value.ToString();
        }

        /// <summary>
        /// Рейтинг пользователей
        /// </summary>
        /// <returns>DataSet</returns>
        public static DataSet GetUserRating()
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            string sqlQuery = @"SELECT full_name, knowledge, reputation
                                FROM users
                                WHERE is_active
                                ORDER BY knowledge DESC
                                LIMIT 10;";
            using MySqlCommand command = new MySqlCommand(sqlQuery, connection);
            using MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }
    }
}
