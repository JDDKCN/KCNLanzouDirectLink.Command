using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace KCNLanzouDirectLink.Command.Options;

/// <summary>
/// 获取文件信息命令选项
/// </summary>
[Verb("info", HelpText = "获取蓝奏云分享链接的文件信息")]
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class GetInfoOptions
{
    public GetInfoOptions() { }

    [Option('u', "url", Required = true, HelpText = "蓝奏云分享链接")]
    public string Url { get; set; } = string.Empty;

    [Option('p', "password", Required = false, HelpText = "分享密码（如果有）")]
    public string? Password { get; set; }

    [Option('j', "json", Required = false, Default = false, HelpText = "以JSON格式输出")]
    public bool JsonOutput { get; set; }
}