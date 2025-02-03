namespace Backend.Dtos.File
{
    public class FileDto
    {
        public int Id { get; set; }
        public string Cover { get; set; } = "";
        public string State { get; set; } = "";
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int Procedures { get; set; }
        public string UserFullName { get; set; } = "";
    }
}
