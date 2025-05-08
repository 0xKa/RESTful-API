using Student_API_Project.Model;
using System.Net.Http.Json;

class Programm
{
    static readonly HttpClient httpClient = new HttpClient();

    public static async Task Main()
    {
        httpClient.BaseAddress = new Uri("http://localhost:5210/api/Students/");

        //await GetAllStudents();
        //await GetPassedStudents();
        //await GetFailedStudents();

        Console.WriteLine("Average Grade: " + await GetAverageGrade());
    }

    static async Task GetAllStudents()
    {
        try
        {
            Console.WriteLine("\nFetching All Students...");
            var students = await httpClient.GetFromJsonAsync<List<Student>>("all");

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
  
    static async Task GetPassedStudents()
    {
        try
        {
            Console.WriteLine("\nFetching Passed Students...");
            var students = await httpClient.GetFromJsonAsync<List<Student>>("Passed");

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

    static async Task GetFailedStudents()
    {
        try
        {
            Console.WriteLine("\nFetching Failed Students...");
            var students = await httpClient.GetFromJsonAsync<List<Student>>("Failed");

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

    static async Task<double?> GetAverageGrade()
    {
        try
        {
            double? avgGrade = await httpClient.GetFromJsonAsync<double?>("Average");

            if (avgGrade != null)
            {
                return avgGrade;

            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Message: " + ex.Message);
        }
            
        return null;

    }



}