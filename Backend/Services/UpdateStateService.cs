using Backend.Dtos.File;
using Backend.Enums;
using Backend.Interfaces;

namespace Backend.Services
{
    public class UpdateStateService(IFileRepository repo, IStateChanger stateChanger) : IUpdateStateService
    {
        public async Task UpdateState(int fileId, string userId)
        {
            var file = await repo.GetByIdAsync(fileId);
            if (file is not null && file.Procedures.Count > 0)
            {
                FileState? newState = stateChanger.GetState(file.Procedures.Last().Label);
                if (newState is not null)
                {
                    var fileDto = new RequestFileDto { Cover = file.Cover, State = newState.Value };
                    await repo.UpdateAsync(fileId, fileDto, userId);
                }
            }
        }
    }
}
