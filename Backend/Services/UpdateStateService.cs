using Backend.Dtos.File;
using Backend.Enums;
using Backend.Interfaces;

namespace Backend.Services
{
    public class UpdateStateService(IFileRepository fileRepo, IStateChanger stateChanger) : IUpdateStateService
    {
        public async Task UpdateState(int fileId, string userId)
        {
            var file = await fileRepo.GetByIdAsync(fileId);
            if (file is not null && file.Procedures.Count > 0)
            {
                FileState? newState = stateChanger.GetState(file.Procedures.Last().Label);
                if (newState is not null)
                {
                    var fileDto = new RequestFileDto { Cover = file.Cover, State = newState.Value };
                    await fileRepo.UpdateAsync(fileId, fileDto, userId);
                }
            }
        }
    }
}
