namespace AppFinalAv4A.Core.Network;

public interface IAccessTokenProvider
{
    Task<string?> GetAccessTokenAsync();
}
