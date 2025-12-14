using MySql.Data.MySqlClient;
using stepik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stepik.Services
{
    public class UsersService
    {
        public static User Get(string fullName)
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
			connection.Open();

			string sqlQuery = "SELECT * FROM users WHERE full_name = @fullName AND is_active = TRUE;";
			
			using var command = new MySqlCommand(sqlQuery, connection);

			command.Parameters.AddWithValue("@fullName", fullName);
			using var reader = command.ExecuteReader();

			while (reader.Read())
			{
				User user = new User
				{
					FullName = reader.GetString(1),
					Details = reader.IsDBNull(2) ? null : reader.GetString(2),
					JoinDate = reader.GetDateTime(3),
					Avatar = reader.IsDBNull(4) ? null : reader.GetString(4),
					IsActive = reader.GetBoolean(5),
				};
				return user;
			}
			return null!;
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
    }
}
