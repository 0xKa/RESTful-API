using DAL.DTO;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL.Data
{
    public class StudentData
    {
        private static string _ConnectionString = "Server=.;Database=StudentDB;User Id=sa;Password=sa123456; Encrypt=False;"; //Microsoft.Data.SqlClient tries to encrypt the connection — even for local databases — and if SQL Server doesn’t have a trusted certificate, the login will fail.

        public static List<StudentDTO> GetAllStudents()
        {
            var students = new List<StudentDTO>();

            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            using (SqlCommand cmd = new SqlCommand("GetAllStudents", conn)) //stored procedure name
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new StudentDTO
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            BirthDate = reader.IsDBNull(reader.GetOrdinal("BirthDate")) ? null : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                            Grade = reader.IsDBNull(reader.GetOrdinal("Grade")) ? null : reader.GetDecimal(reader.GetOrdinal("Grade")),
                            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                            IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? null : reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        });
                    }
                }
            }

            return students;
        }

        public static int GetStudentCount()
        {
            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Student", conn))
            {
                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public static List<StudentDTO> GetPassedFailedStudents(bool isPassed)
        {
            var students = new List<StudentDTO>();

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
                        students.Add(new StudentDTO
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            BirthDate = reader.IsDBNull(reader.GetOrdinal("BirthDate")) ? null : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                            Grade = reader.IsDBNull(reader.GetOrdinal("Grade")) ? null : reader.GetDecimal(reader.GetOrdinal("Grade")),
                            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                            IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? null : reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        });
                    }
                }
            }

            return students;
        }

        public static double GetAverageGrade()
        {
            double result;

            using (SqlConnection conn = new SqlConnection(_ConnectionString))
            using (SqlCommand cmd = new SqlCommand("GetAverageGrade", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                result = Convert.ToDouble(cmd.ExecuteScalar());

            }

            return result;
        }

        public static StudentDTO? GetStudentByID(int id)
        {
            StudentDTO? student = null;

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
                        student = new StudentDTO
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            BirthDate = reader.IsDBNull(reader.GetOrdinal("BirthDate")) ? null : reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                            Grade = reader.IsDBNull(reader.GetOrdinal("Grade")) ? null : reader.GetDecimal(reader.GetOrdinal("Grade")),
                            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                            IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? null : reader.GetBoolean(reader.GetOrdinal("IsActive"))
                        };
                    }
                }
            }

            return student;
        }

    }
}
