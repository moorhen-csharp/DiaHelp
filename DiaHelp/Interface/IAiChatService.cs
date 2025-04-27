namespace DiaHelp.Interface
{
    public interface IAiChatService
    {
        Task<string> GetDiabetesResponseAsync(string message);
    }
}
