using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace KCNLanzouDirectLink.Command.Options;

/// <summary>
/// 获取直链命令选项
/// </summary>
[Verb("link", HelpText = "获取蓝奏云分享链接的直链")]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class GetLinkOptions
{
    public GetLinkOptions() { }

    [Option('u', "url", Required = true, HelpText = "蓝奏云分享链接")]
    public string Url { get; set; } = string.Empty;

    [Option('p', "password", Required = false, HelpText = "分享密码（如果有）")]
    public string? Password { get; set; }

    [Option('r', "retry", Required = false, Default = 3, HelpText = "重试次数（仅对加密链接有效）")]
    public int RetryCount { get; set; }

    [Option('j', "json", Required = false, Default = false, HelpText = "以JSON格式输出")]
    public bool JsonOutput { get; set; }

    [Option('q', "quiet", Required = false, Default = false, HelpText = "静默模式，仅输出直链")]
    public bool Quiet { get; set; }
}