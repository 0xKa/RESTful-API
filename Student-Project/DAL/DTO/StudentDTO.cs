namespace DAL.DTO
{
    public class StudentDTO
    {
        public required int ID { get; set; }
        public required string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public decimal? Grade { get; set; }
        public string? Email { get; set; }
        public bool? IsActive { get; set; }
    }
}
