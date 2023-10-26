using Gateway.Models;

namespace Gateway.Services
{
    public interface ILogService
    {
        Task AddLog(long idText, int state);
        void CreateLog(long idText);
        Task<IEnumerable<Log>> GetLog(int textId);
    }
}