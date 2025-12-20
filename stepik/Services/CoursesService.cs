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
        public static int GetTotalCount()
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();

            string sqlQuery = "SELECT COUNT(id) FROM courses";

            using var command = new MySqlCommand(sqlQuery, connection);
            object totalCount = command.ExecuteScalar();
            return Convert.ToInt32(totalCount);
        }

        public static List<Course> Get(string fullName)
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();

            string sqlQuery = @"SELECT c.title, c.summary, c.photo
                                FROM courses AS c JOIN users AS us ON us.full_name = @FullName
                                JOIN user_courses AS uc ON uc.user_id = us.id 
                                WHERE c.id = uc.course_id
                                ORDER BY uc.last_viewed DESC;";

            using var command = new MySqlCommand(sqlQuery, connection);
            var fullNameParam = new MySqlParameter("@FullName", fullName);
            command.Parameters.Add(fullNameParam);
            
            using var reader = command.ExecuteReader();
            List<Course> courseList = new List<Course>();

            while (reader.Read())
            {
                Course course = new Course
                {
                    Title = reader.GetString(1),
                    Summary = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Photo = reader.IsDBNull(4) ? null : reader.GetString(4),
                };

                courseList.Add(course);
            }

            return courseList;
        }
    }
}
