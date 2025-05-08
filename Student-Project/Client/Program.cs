using Student_API_Project.Model;
using System.Net;
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

        //Console.WriteLine("Average Grade: " + await GetAverageGrade());

        //Console.WriteLine("Enter Student ID:");
        //Student? student = await GetStudentByID(Convert.ToInt32(Console.ReadLine()));
        //student?.PrintCard();

        //await AddStudent(new Student { Name = "Issa", BirthDate = new DateTime(2000, 3, 3), Grade = 44 });


        await DeleteStudent(3);
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

    static async Task<Student?> GetStudentByID(int ID)
    {
        Console.WriteLine($"\nFetching student with ID {ID}...\n");

        try
        {
            //Student? student = await httpClient.GetFromJsonAsync<Student>(ID.ToString());

            //if (student != null)
            //    return student;

            // OR

            var response = await httpClient.GetAsync($"{ID}");

            if (response.IsSuccessStatusCode)
            {
                var student = await response.Content.ReadFromJsonAsync<Student>();
                if (student != null)
                {
                    return student;
                }
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                Console.WriteLine($"Bad Request: Not accepted ID {ID}");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Not Found: Student with ID {ID} not found.");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Message: " + ex.Message);
        }

        return null;
    }

    static async Task AddStudent(Student newStudent)
    {
        Console.WriteLine($"\nAdding New Student...\n");

        try
        {
            var response = await httpClient.PostAsJsonAsync("AddNewStudent", newStudent);

            if (response.IsSuccessStatusCode)
            {
                var addedStudent = await response.Content.ReadFromJsonAsync<Student>();
                Console.WriteLine("Student Added");
                addedStudent?.PrintCard();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                Console.WriteLine("Bad Request: Invalid student data");
            else if (response.StatusCode == HttpStatusCode.NotFound)
                Console.WriteLine("404 Not Found");

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

    }

    static async Task DeleteStudent(int StudentID)
    {
        Console.WriteLine($"\nDeleting Student ID...\n");

        try
        {
            var response = await httpClient.DeleteAsync($"Delete/{StudentID}");

            if (response.IsSuccessStatusCode)
                Console.WriteLine($"Student with ID {StudentID} Deleted.");
            else if (response.StatusCode == HttpStatusCode.BadRequest)
                Console.WriteLine("Bad Request: Invalid student data");
            else if (response.StatusCode == HttpStatusCode.NotFound)
                Console.WriteLine("404 Not Found");

        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

    }



}