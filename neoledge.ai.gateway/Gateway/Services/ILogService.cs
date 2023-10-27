using Gateway.Models;

namespace Gateway.Services
{
    public interface ILogService
    {
        Task AddLog(long idText, int state);
        Task CreateLog(long idText, DateTime date);
        Task<IEnumerable<Log>> GetLog(int textId);
    }
}