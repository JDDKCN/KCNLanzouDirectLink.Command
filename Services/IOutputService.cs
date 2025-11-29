namespace KCNLanzouDirectLink.Command.Services;

/// <summary>
/// 输出服务接口
/// </summary>
public interface IOutputService
{
    void WriteInfo(string message);
    void WriteSuccess(string message);
    void WriteWarning(string message);
    void WriteError(string message);
    void WriteLine(string message);
    void WriteLine();
    void WriteJson<T>(T obj);
    void WriteProgress(int current, int total, string message);
}