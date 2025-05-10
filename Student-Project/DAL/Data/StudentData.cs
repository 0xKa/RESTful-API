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

            return student;
        }

        public static int AddNewStudent(StudentDTO student)
        {
            int newStudentId = -1;

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

            return newStudentId;
        }

        public static bool UpdateStudent(StudentDTO student)
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


        public static bool DeleteStudent(int id)
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


        public static bool IsStudentExists(int id)
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
    }
}
