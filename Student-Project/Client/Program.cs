using Student_API_Project.Model;
using System.Net.Http.Json;

class Programm
{
    static readonly HttpClient httpClient = new HttpClient();

    public static async Task Main()
    {
        httpClient.BaseAddress = new Uri("http://localhost:5210/api/Students/");

        await GetAllStudents();

    }

    static async Task GetAllStudents()
    {
        try
        {
            Console.WriteLine("\nFetching All Students...");
            var students = await httpClient.GetFromJsonAsync<List<Student>>("AllStudents");

            if (students != null)
            {
                foreach (var student in students)
                {
                    student.PrintCard();
                }

            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Message: " + ex.Message);
        }


    }
}