using Azure;
using Azure.Storage.Files.Shares;
using System.Data.SqlClient;
using System.Security.Claims;
using ProgPoe.Models;

namespace ProgPoe.Models
{
    public class claimOps
    {
        public int LecturerID { get; set; }
        public int HoursWorked { get; set; }
        public int HourlyRate { get; set; }
        public string ClaimStatus { get; set; }
        public int DocumentID { get; set; }
        public string AdminComment { get; set; }
        public string UniName { get; set; }

        public static string connectionString = "Server=tcp:progpoeserver.database.windows.net,1433;Initial Catalog=progpoedatabase;Persist Security Info=False;User ID=ryanv2004;Password=AceVents12!@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public SqlConnection con = new SqlConnection(connectionString);

        public int SaveClaimToDatabase()
        {
            try
            {
                string sql = "INSERT INTO Claims (lecturerID, hoursWorked, hourlyRate, claimStatus, uniName) OUTPUT INSERTED.claimID VALUES (@lecturerID, @hoursWorked, @hourlyRate, @claimStatus, @uniName)";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@lecturerID", LecturerID);
                cmd.Parameters.AddWithValue("@hoursWorked", HoursWorked);
                cmd.Parameters.AddWithValue("@hourlyRate", HourlyRate);
                cmd.Parameters.AddWithValue("@claimStatus", ClaimStatus);
                cmd.Parameters.AddWithValue("@uniName", UniName);

                con.Open();
                int claimID = (int)cmd.ExecuteScalar();
                con.Close();

                return claimID;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                return -1;
            }
        }

        public async Task<string> UploadDocumentToAzureFileShare(IFormFile document, int claimID)
        {
            try
            {
                string connectionString = "DefaultEndpointsProtocol=https;AccountName=st10365052progpoe;AccountKey=T1MGI+JNm1YQO02vxwnHZmbUz0WssV5YDWaQOijfLF/KXqoPo7pJZ1LEH4I8EtfZtaeVQpGGQXhd+AStwkNuJA==;EndpointSuffix=core.windows.net";
                string shareName = "st10365052fileshare";
                string directoryName = "claims";
                string fileName = $"{claimID}_{document.FileName}";

                ShareClient share = new ShareClient(connectionString, shareName);
                ShareDirectoryClient directory = share.GetDirectoryClient(directoryName);

                // Ensure the directory exists
                await directory.CreateIfNotExistsAsync();

                ShareFileClient file = directory.GetFileClient(fileName);

                using (var stream = document.OpenReadStream())
                {
                    await file.CreateAsync(stream.Length);
                    await file.UploadRangeAsync(new HttpRange(0, stream.Length), stream);
                }

                return file.Uri.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading document: {ex.Message}");
                return null;
            }
        }

        public int SaveDocumentDetailsToDatabase(int lecturerID, string uploadURL)
        {

            try
            {

                string sql = "INSERT INTO UserDocuments (lecturerID, uploadURL) VALUES (@lecturerID, @uploadURL)";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@lecturerID", lecturerID);
                cmd.Parameters.AddWithValue("@uploadURL", uploadURL);

                con.Open();
                int documentID = (int)cmd.ExecuteScalar();
                con.Close();

                return documentID;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                return -1;
            }
        }

        public bool UpdateClaimWithDocumentID(int claimID, int documentID)
        {
            try
            {
                string sql = "UPDATE Claims SET documentID = @documentID WHERE claimID = @claimID";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@documentID", documentID);
                cmd.Parameters.AddWithValue("@claimID", claimID);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SaveClaimAndDocument(IFormFile document)
        {
            // Save the claim to the database and get the claimID
            int claimID = SaveClaimToDatabase();

            // Upload the document to Azure File Share and get the URL
            string uploadURL = await UploadDocumentToAzureFileShare(document, claimID);

            // Save the document details to the database and get the documentID
            int documentID = SaveDocumentDetailsToDatabase(LecturerID, uploadURL);

            // Update the claim with the documentID
            bool updateSuccess = UpdateClaimWithDocumentID(claimID, documentID);
            return updateSuccess;
        }

        public List<dClaim> GetClaimsFromDatabase()
        {
            List<dClaim> claims = new List<dClaim>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sql = @"
                    SELECT 
                        L.firstName, 
                        L.lastName, 
                        C.hoursWorked, 
                        U.uploadURL AS DocumentURL, 
                        C.claimStatus AS Status
                    FROM 
                        Claims C
                    INNER JOIN 
                        Lecturers L ON C.lecturerID = L.lecturerID
                    INNER JOIN 
                        UserDocuments U ON C.documentID = U.documentID";
                    SqlCommand cmd = new SqlCommand(sql, con);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        dClaim claim = new dClaim
                        {
                            FirstName = reader["firstName"].ToString(),
                            LastName = reader["lastName"].ToString(),
                            HoursWorked = Convert.ToInt32(reader["hoursWorked"]),
                            DocumentURL = reader["DocumentURL"].ToString(),
                            Status = reader["Status"].ToString()
                        };
                        claims.Add(claim);
                    }

                    con.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }

            return claims;
        }

        public List<dClaim> GetClaimsFromDatabaseAdmin()
        {
            List<dClaim> claims = new List<dClaim>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sql = @"
            SELECT 
                C.claimID,
                L.firstName, 
                L.lastName, 
                C.hoursWorked, 
                U.uploadURL AS DocumentURL, 
                C.claimStatus AS Status,
                C.adminComment
            FROM 
                Claims C
            INNER JOIN 
                Lecturers L ON C.lecturerID = L.lecturerID
            INNER JOIN 
                UserDocuments U ON C.documentID = U.documentID";
                    SqlCommand cmd = new SqlCommand(sql, con);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        dClaim claim = new dClaim
                        {
                            ClaimID = Convert.ToInt32(reader["claimID"]),
                            FirstName = reader["firstName"].ToString(),
                            LastName = reader["lastName"].ToString(),
                            HoursWorked = Convert.ToInt32(reader["hoursWorked"]),
                            DocumentURL = reader["DocumentURL"].ToString(),
                            Status = reader["Status"].ToString(),
                            AdminComment = reader["adminComment"].ToString()
                        };
                        claims.Add(claim);
                    }

                    con.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }

            return claims;
        }

        public bool UpdateClaimStatusAndComment(int claimID, string status, string comment)
        {
            try
            {
                string sql = "UPDATE Claims SET claimStatus = @status, adminComment = @comment WHERE claimID = @claimID";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@comment", comment);
                cmd.Parameters.AddWithValue("@claimID", claimID);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                return false;
            }
        }


    }
}


