using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stepik.Services
{
    public class CertificatesService
    {
        /// <summary>
        /// Получение сертификатов пользователя
        /// </summary>
        /// <param name="fullName">Полное имя пользователя</param>
        /// <returns>DataSet</returns>
        public static DataSet Get(string fullName)
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();
            string sqlGetCertificatesQuery = @"SELECT courses.title, cert.issue_date, cert.grade
                                               FROM certificates AS cert
                                               JOIN users ON users.full_name = @FullName
                                               JOIN courses ON courses.id = cert.course_id
                                               WHERE cert.user_id = users.id
                                               ORDER BY cert.issue_date DESC;";
            using var getCertificatesCommand = new MySqlCommand(sqlGetCertificatesQuery, connection);
            var fullNameParam = new MySqlParameter("@FullName", fullName);
            getCertificatesCommand.Parameters.Add(fullNameParam);

            using var dataAdapter = new MySqlDataAdapter(getCertificatesCommand);
            var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            return dataSet;
        }
    }
}
