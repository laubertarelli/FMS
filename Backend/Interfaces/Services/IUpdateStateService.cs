namespace Backend.Interfaces
{
    public interface IUpdateStateService
    {
        public Task UpdateState(int fileId, string userId);
    }
}
