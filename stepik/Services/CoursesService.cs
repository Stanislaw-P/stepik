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
    public class CoursesService
    {
        public int GetTotalCount()
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();

            string sqlQuery = "SELECT COUNT(id) FROM courses";

            using var command = new MySqlCommand(sqlQuery, connection);
            object totalCount = command.ExecuteScalar();
            return Convert.ToInt32(totalCount);
        }

        public List<Course> Get(string fullName)
        {
            var courses = new List<Course>();

            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();

            var query = @"SELECT title, summary, photo, courses.id
                      FROM user_courses
                      JOIN courses ON user_courses.course_id = courses.id
                      JOIN users ON users.id = user_courses.user_id
                      WHERE users.full_name = @fullName AND users.is_active = 1
                      ORDER BY user_courses.last_viewed DESC;";

            using var command = new MySqlCommand(query, connection);
            var fullNameParam = new MySqlParameter("@fullName", fullName);
            command.Parameters.Add(fullNameParam);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var course = new Course
                {
                    Id = reader.GetInt32("id"),
                    Title = reader.GetString("title"),
                    Summary = reader.IsDBNull("summary") ? null : reader.GetString("summary"),
                    Photo = reader.IsDBNull("photo") ? null : reader.GetString("photo")
                };
                courses.Add(course);
            }

            return courses;
        }
    }
}
