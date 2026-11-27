namespace AppFinalAv4A.Core.Network;

public static class ApiConfig
{

    public const string BaseUrl = "http://192.168.1.87:8080/";

    public static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(20);
}
