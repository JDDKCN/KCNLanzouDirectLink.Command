using System.Text.Json;
using System.Text.Json.Serialization;

namespace KCNLanzouDirectLink.Command.Services;

/// <summary>
/// 控制台输出服务实现
/// </summary>
public class ConsoleOutputService : IOutputService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new JsonStringEnumConverter() }
    };

    public void WriteInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[INFO] {message}");
        Console.ResetColor();
    }

    public void WriteSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[SUCCESS] {message}");
        Console.ResetColor();
    }

    public void WriteWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARNING] {message}");
        Console.ResetColor();
    }

    public void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {message}");
        Console.ResetColor();
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public void WriteLine()
    {
        Console.WriteLine();
    }

    public void WriteJson<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj, JsonOptions);
        Console.WriteLine(json);
    }

    public void WriteProgress(int current, int total, string message)
    {
        var percentage = (int)((double)current / total * 100);
        var progressBar = new string('█', percentage / 5) + new string('░', 20 - percentage / 5);
        Console.Write($"\r[{progressBar}] {percentage}% ({current}/{total}) {message}");

        if (current == total)
        {
            Console.WriteLine();
        }
    }
}