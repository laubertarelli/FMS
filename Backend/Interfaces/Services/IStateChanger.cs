using Backend.Enums;

namespace Backend.Interfaces
{
    public interface IStateChanger
    {
        public FileState? GetState(ProcedureLabel label);
    }
}
