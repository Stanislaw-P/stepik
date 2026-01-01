using MySql.Data.MySqlClient;
using stepik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stepik.Services
{
    public class CommentsService
    {
        public List<Comment> Get(int courseId)
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();

            string sqlQuery = """
                SELECT c.id, c.text, c.time 
                FROM comments AS c
                JOIN steps AS s ON c.step_id = s.id 
                JOIN unit_lessons AS ul ON s.lesson_id = ul.lesson_id
                JOIN units AS u ON ul.unit_id = u.id
                JOIN courses AS cr ON u.course_id = cr.id
                WHERE reply_comment_id IS NULL AND cr.id = @id ORDER BY c.time DESC;
                """;

            using var command = new MySqlCommand(sqlQuery, connection);

            var idParap = new MySqlParameter("@id", courseId);
            command.Parameters.Add(idParap);

            using var reader = command.ExecuteReader();

            var comments = new List<Comment>();

            while (reader.Read())
            {
                var id = reader.GetInt32("id");
                var text = reader.GetString("text");
                var time = reader.GetDateTime("time");

                Comment comment = new Comment
                {
                    Id = id,
                    Text = text,
                    Time = time
                };
                comments.Add(comment);
            }

            return comments;
        }

        /// <summary>
        /// Удаление комментария пользователя
        /// </summary>
        /// <param name="id">id комментария</param>
        /// <returns>Удалось ли удалить комментарий</returns>
        public bool Delete(int id)
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                string sqlQuery = "DELETE FROM course_reviews WHERE comment_id = @id;";
                using var command = new MySqlCommand(sqlQuery, connection, transaction);
                command.Parameters.AddWithValue("@id", id)  ;
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM comments WHERE reply_comment_id = @id;";
                command.ExecuteNonQuery();

                command.CommandText = "DELETE FROM comments WHERE id = @id;";
                command.ExecuteNonQuery();

                transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Transaction failed: " + ex.Message);
                return false;
            }
        }
    }
}
