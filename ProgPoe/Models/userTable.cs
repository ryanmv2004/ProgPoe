using Microsoft.AspNetCore.Hosting.Server;
using System.Data.SqlClient;

namespace ProgPoe.Models
{
    public class userTable
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string uniName { get; set; }

        private static string connectionString = "Server=tcp:progpoeserver.database.windows.net,1433;Initial Catalog=progpoedatabase;Persist Security Info=False;User ID=ryanv2004;Password=AceVents12!@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public int insert_User(userTable m)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sql = "INSERT INTO Lecturers (firstName, lastName, email, password, uniName) VALUES (@firstname,@lastname,@email,@password,@uniname)";
                    SqlCommand cmd = new SqlCommand(sql, con);

                    cmd.Parameters.AddWithValue("@firstname", m.firstName);
                    cmd.Parameters.AddWithValue("@lastname", m.lastName);
                    cmd.Parameters.AddWithValue("@email", m.email);
                    cmd.Parameters.AddWithValue("@password", m.password);
                    cmd.Parameters.AddWithValue("@uniname", m.uniName);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }

        public int FetchUser(string email, string password)
        {
            int userID = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT lecturerID FROM Lecturers WHERE email = @Email AND password = @Password";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();

                if (result != null && result != DBNull.Value)
                {
                    userID = Convert.ToInt32(result);
                }
            }
            return userID;
        }

        public int FetchUserAdmin(string email, string password)
        {
            int userID = -1;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT uniAdminID FROM claimsAdmin WHERE email = @Email AND password = @Password";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                con.Open();
                object result = cmd.ExecuteScalar();
                con.Close();

                if (result != null && result != DBNull.Value)
                {
                    userID = Convert.ToInt32(result);
                }
            }
            return userID;
        }

        public int insert_UserAdmin(userTable m)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sql = "INSERT INTO claimsAdmin (firstName, lastName, email, password, uniName) VALUES (@firstname,@lastname,@email,@password,@uniname)";
                    SqlCommand cmd = new SqlCommand(sql, con);

                    cmd.Parameters.AddWithValue("@firstname", m.firstName);
                    cmd.Parameters.AddWithValue("@lastname", m.lastName);
                    cmd.Parameters.AddWithValue("@email", m.email);
                    cmd.Parameters.AddWithValue("@password", m.password);
                    cmd.Parameters.AddWithValue("@uniname", m.uniName);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }

        public List<dLecturer> getLecturerList()
        {
            List<dLecturer> lecturerList = new List<dLecturer>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Lecturers";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    dLecturer lecturer = new dLecturer
                    {
                        firstName = rdr["firstName"].ToString(),
                        lastName = rdr["lastName"].ToString(),
                        email = rdr["email"].ToString(),
                        password = rdr["password"].ToString(),
                        uniName = rdr["uniName"].ToString()
                    };
                    lecturerList.Add(lecturer);
                }
                con.Close();
            }
            return lecturerList;
        }

        public int UpdateLecturer(dLecturer lecturer)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sql = "UPDATE Lecturers SET firstName = @firstName, lastName = @lastName, password = @password, uniName = @uniName WHERE email = @Email";
                    SqlCommand cmd = new SqlCommand(sql, con);

                    cmd.Parameters.AddWithValue("@firstName", lecturer.firstName);
                    cmd.Parameters.AddWithValue("@lastName", lecturer.lastName);
                    cmd.Parameters.AddWithValue("@password", lecturer.password);
                    cmd.Parameters.AddWithValue("@uniName", lecturer.uniName);
                    cmd.Parameters.AddWithValue("@Email", lecturer.email);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }
    }
}
