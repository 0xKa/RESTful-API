using DAL.DTO;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL.Data
{
    public class StudentData
    {
        private static string _ConnectionString = "Server=.;Database=StudentDB;User Id=sa;Password=sa123456; Encrypt=False;"; //Microsoft.Data.SqlClient tries to encrypt the connection — even for local databases — and if SQL Server doesn’t have a trusted certificate, the login will fail.

        public static StudentDTO? GetStudentByID(int id)
        {
            StudentDTO? student = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_ConnectionString))
                using (SqlCommand cmd = new SqlCommand("GetStudentByID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            student = new StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("ID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.IsDBNull(reader.GetOrdinal("BirthDate")) ? null : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                                reader.IsDBNull(reader.GetOrdinal("Grade")) ? null : reader.GetDecimal(reader.GetOrdinal("Grade")),
                                reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                reader.IsDBNull(reader.GetOrdinal("IsActive")) ? null : reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // we can log this exception or rethrow it
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            return student;
        }

        public static int AddNewStudent(StudentDTO student)
        {
            int newStudentId = -1;

            try
            {
                using (SqlConnection conn = new SqlConnection(_ConnectionString))
                using (SqlCommand cmd = new SqlCommand("AddNewStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@BirthDate", (object?)student.BirthDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Grade", (object?)student.Grade ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object?)student.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", (object?)student.IsActive ?? false);

                    var outputIdParam = new SqlParameter("@NewStudentID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputIdParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    newStudentId = (int)outputIdParam.Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in AddNewStudent: " + ex.Message);
            }

            return newStudentId;
        }

        public static bool UpdateStudent(StudentDTO student)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_ConnectionString))
                using (SqlCommand cmd = new SqlCommand("UpdateStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", student.ID);
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@BirthDate", (object?)student.BirthDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Grade", (object?)student.Grade ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object?)student.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", (object?)student.IsActive ?? true);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateStudent: " + ex.Message);
                return false;
            }
        }

        public static bool DeleteStudent(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_ConnectionString))
                using (SqlCommand cmd = new SqlCommand("DeleteStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in DeleteStudent: " + ex.Message);
                return false;
            }
        }

        public static List<StudentDTO> GetAllStudents()
        {
            var students = new List<StudentDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_ConnectionString))
                using (SqlCommand cmd = new SqlCommand("GetAllStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("ID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.IsDBNull(reader.GetOrdinal("BirthDate")) ? null : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                                reader.IsDBNull(reader.GetOrdinal("Grade")) ? null : reader.GetDecimal(reader.GetOrdinal("Grade")),
                                reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                reader.IsDBNull(reader.GetOrdinal("IsActive")) ? null : reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetAllStudents: " + ex.Message);
            }

            return students;
        }

        public static List<StudentDTO> GetPassedFailedStudents(bool isPassed)
        {
            var students = new List<StudentDTO>();

            try
            {
                using (SqlConnection conn = new SqlConnection(_ConnectionString))
                using (SqlCommand cmd = new SqlCommand("GetPassedFailedStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IsPassed", isPassed);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("ID")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.IsDBNull(reader.GetOrdinal("BirthDate")) ? null : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                                reader.IsDBNull(reader.GetOrdinal("Grade")) ? null : reader.GetDecimal(reader.GetOrdinal("Grade")),
                                reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                reader.IsDBNull(reader.GetOrdinal("IsActive")) ? null : reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            ));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetPassedFailedStudents: " + ex.Message);
            }

            return students;
        }

        public static bool IsStudentExists(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_ConnectionString))
                using (SqlCommand cmd = new SqlCommand("IsStudentExists", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    return Convert.ToInt32(result) == 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in IsStudentExists: " + ex.Message);
                return false;
            }
        }

        public static int GetStudentCount()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_ConnectionString))
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Student", conn))
                {
                    conn.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetStudentCount: " + ex.Message);
                return -1;
            }
        }

        public static double GetAverageGrade()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_ConnectionString))
                using (SqlCommand cmd = new SqlCommand("GetAverageGrade", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    return Convert.ToDouble(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetAverageGrade: " + ex.Message);
                return -1;
            }
        }

    }
}
