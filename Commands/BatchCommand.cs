using KCNLanzouDirectLink.Command.Options;
using KCNLanzouDirectLink.Command.Services;
using KCNLanzouDirectLink.Command.Utils;
using KCNLanzouDirectLink.Models;
using System.Text;
using System.Text.Json;

namespace KCNLanzouDirectLink.Command.Commands;

/// <summary>
/// 批量获取命令
/// </summary>
public class BatchCommand : ICommand<BatchOptions>
{
    private readonly IOutputService _output;

    public BatchCommand(IOutputService outputService)
    {
        _output = outputService ?? throw new ArgumentNullException(nameof(outputService));
    }

    public async Task<int> ExecuteAsync(BatchOptions options)
    {
        try
        {
            if (!File.Exists(options.FilePath))
            {
                _output.WriteError($"文件不存在: {options.FilePath}");
                return 1;
            }

            var lines = await File.ReadAllLinesAsync(options.FilePath);
            var urlItems = ParseUrlFile(lines);

            if (urlItems.Count == 0)
            {
                _output.WriteWarning("文件中没有有效的链接");
                return 1;
            }

            if (!options.JsonOutput)
            {
                _output.WriteInfo($"共找到 {urlItems.Count} 个链接，开始处理...");
                _output.WriteLine();
            }

            var results = new List<BatchResultItem>();
            var successCount = 0;
            var failCount = 0;

            for (int i = 0; i < urlItems.Count; i++)
            {
                var (url, password) = urlItems[i];

                if (!options.JsonOutput)
                {
                    _output.WriteProgress(i + 1, urlItems.Count, $"处理: {TruncateUrl(url)}");
                }

                var (state, directLink) = string.IsNullOrWhiteSpace(password)
                    ? await KCNLanzouLinkHelper.GetDirectLinkAsync(url)
                    : await KCNLanzouLinkHelper.GetDirectLinkAsync(url, password, options.RetryCount);

                var resultItem = new BatchResultItem
                {
                    SourceUrl = url,
                    HasPassword = !string.IsNullOrWhiteSpace(password),
                    State = state.ToString(),
                    StateDescription = ResultFormatter.GetStateDescription(state),
                    DirectLink = directLink,
                    Success = state == DownloadState.Success
                };

                results.Add(resultItem);

                if (state == DownloadState.Success)
                {
                    successCount++;
                }
                else
                {
                    failCount++;
                }

                // 添加延迟避免请求过快
                if (i < urlItems.Count - 1 && options.DelayMs > 0)
                {
                    await Task.Delay(options.DelayMs);
                }
            }

            // 输出结果
            if (options.JsonOutput)
            {
                _output.WriteJson(new
                {
                    TotalCount = urlItems.Count,
                    SuccessCount = successCount,
                    FailCount = failCount,
                    Results = results
                });
            }
            else
            {
                _output.WriteLine();
                _output.WriteLine();
                _output.WriteInfo($"处理完成! 成功: {successCount}, 失败: {failCount}");
                _output.WriteLine();

                // 输出详细结果
                foreach (var result in results)
                {
                    if (result.Success)
                    {
                        _output.WriteSuccess($"{result.SourceUrl}");
                        _output.WriteLine($"  -> {result.DirectLink}");
                    }
                    else
                    {
                        _output.WriteError($"{result.SourceUrl}");
                        _output.WriteLine($"  -> {result.StateDescription}");
                    }
                }
            }

            // 写入输出文件
            if (!string.IsNullOrWhiteSpace(options.OutputPath))
            {
                await WriteOutputFileAsync(options.OutputPath, results, options.JsonOutput);

                if (!options.JsonOutput)
                {
                    _output.WriteLine();
                    _output.WriteSuccess($"结果已保存到: {options.OutputPath}");
                }
            }

            return failCount > 0 ? 1 : 0;
        }
        catch (Exception ex)
        {
            if (options.JsonOutput)
            {
                _output.WriteJson(new
                {
                    Success = false,
                    Error = ex.Message
                });
            }
            else
            {
                _output.WriteError($"发生异常: {ex.Message}");
            }
            return 1;
        }
    }

    private static List<(string Url, string? Password)> ParseUrlFile(string[] lines)
    {
        var result = new List<(string, string?)>();

        foreach (var line in lines)
        {
            var trimmed = line.Trim();

            // 跳过空行和注释
            if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith('#') || trimmed.StartsWith("//"))
            {
                continue;
            }

            // 支持多种分隔符: 逗号、制表符、空格
            string url;
            string? password = null;

            var commaIndex = trimmed.IndexOf(',');
            var tabIndex = trimmed.IndexOf('\t');
            var spaceIndex = trimmed.LastIndexOf(' ');

            if (commaIndex > 0)
            {
                url = trimmed[..commaIndex].Trim();
                password = trimmed[(commaIndex + 1)..].Trim();
            }
            else if (tabIndex > 0)
            {
                url = trimmed[..tabIndex].Trim();
                password = trimmed[(tabIndex + 1)..].Trim();
            }
            else if (spaceIndex > 0 && !trimmed.Contains("://", StringComparison.Ordinal) ||
                     (spaceIndex > trimmed.IndexOf("://", StringComparison.Ordinal) + 3))
            {
                // 只有当空格不是URL的一部分时才分割
                var lastSpaceIndex = trimmed.LastIndexOf(' ');
                var possiblePassword = trimmed[(lastSpaceIndex + 1)..].Trim();

                // 检查是否像密码（通常是短字符串）
                if (possiblePassword.Length <= 10 && !possiblePassword.Contains('/'))
                {
                    url = trimmed[..lastSpaceIndex].Trim();
                    password = possiblePassword;
                }
                else
                {
                    url = trimmed;
                }
            }
            else
            {
                url = trimmed;
            }

            if (!string.IsNullOrEmpty(url))
            {
                result.Add((url, string.IsNullOrEmpty(password) ? null : password));
            }
        }

        return result;
    }

    private static string TruncateUrl(string url, int maxLength = 50)
    {
        if (url.Length <= maxLength)
        {
            return url;
        }
        return url[..(maxLength - 3)] + "...";
    }

    private static async Task WriteOutputFileAsync(string path, List<BatchResultItem> results, bool asJson)
    {
        if (asJson)
        {
            var json = JsonSerializer.Serialize(results, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            await File.WriteAllTextAsync(path, json, Encoding.UTF8);
        }
        else
        {
            var sb = new StringBuilder();
            sb.AppendLine("# 蓝奏云直链解析结果");
            sb.AppendLine($"# 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine();

            foreach (var result in results)
            {
                if (result.Success)
                {
                    sb.AppendLine($"{result.SourceUrl} -> {result.DirectLink}");
                }
                else
                {
                    sb.AppendLine($"# [失败] {result.SourceUrl} -> {result.StateDescription}");
                }
            }

            await File.WriteAllTextAsync(path, sb.ToString(), Encoding.UTF8);
        }
    }

    private class BatchResultItem
    {
        public string SourceUrl { get; set; } = string.Empty;
        public bool HasPassword { get; set; }
        public string State { get; set; } = string.Empty;
        public string StateDescription { get; set; } = string.Empty;
        public string? DirectLink { get; set; }
        public bool Success { get; set; }
    }
}