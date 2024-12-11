namespace Backend.Interfaces
{
    public interface ITimestampable
    {
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
