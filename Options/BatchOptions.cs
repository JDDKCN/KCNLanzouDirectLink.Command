using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace KCNLanzouDirectLink.Command.Options;

/// <summary>
/// 批量获取命令选项
/// </summary>
[Verb("batch", HelpText = "批量获取蓝奏云分享链接的直链")]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class BatchOptions
{
    public BatchOptions() { }

    [Option('f', "file", Required = true, HelpText = "包含链接的文件路径（每行一个链接，格式：url 或 url,password）")]
    public string FilePath { get; set; } = string.Empty;

    [Option('o', "output", Required = false, HelpText = "输出文件路径")]
    public string? OutputPath { get; set; }

    [Option('r', "retry", Required = false, Default = 3, HelpText = "重试次数（仅对加密链接有效）")]
    public int RetryCount { get; set; }

    [Option('j', "json", Required = false, Default = false, HelpText = "以JSON格式输出")]
    public bool JsonOutput { get; set; }

    [Option('d', "delay", Required = false, Default = 500, HelpText = "每个请求之间的延迟（毫秒）")]
    public int DelayMs { get; set; }
}