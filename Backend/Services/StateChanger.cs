using Backend.Enums;
using Backend.Interfaces;

namespace Backend.Services
{
    public class StateChanger : IStateChanger
    {
        public FileState? GetState(ProcedureLabel label)
        {
            return label switch
            {
                ProcedureLabel.InStudio => FileState.ToSolve,
                ProcedureLabel.Resolution => FileState.Resolved,
                ProcedureLabel.Archived => FileState.Finished,
                _ => null,
            };
        }
    }
}
