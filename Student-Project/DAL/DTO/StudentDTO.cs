namespace DAL.DTO
{
    public class StudentDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? Grade { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }

        public StudentDTO(int ID, string name, DateTime? birthdate, decimal? grade, string? email, bool? isactive)
        {
            this.ID = ID;
            this.Name = name;
            this.BirthDate = birthdate;
            this.Grade = grade;
            this.Email = email;
            this.IsActive = isactive;

        }

    }
}
