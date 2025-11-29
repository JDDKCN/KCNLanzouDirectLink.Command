using KCNLanzouDirectLink.Models;

namespace KCNLanzouDirectLink.Command.Utils;

/// <summary>
/// 结果格式化工具
/// </summary>
public static class ResultFormatter
{
    /// <summary>
    /// 获取状态的友好描述
    /// </summary>
    public static string GetStateDescription(DownloadState state) => state switch
    {
        DownloadState.Success => "成功",
        DownloadState.Error => "发生错误",
        DownloadState.UrlNotProvided => "URL无效或未提供",
        DownloadState.PostsignNotFound => "无法解析加密信息。分享链接无效或密钥错误？",
        DownloadState.HtmlContentNotFound => "无法获取网页内容。分享链接无效？",
        DownloadState.IntermediateUrlNotFound => "无法解析中间链接。",
        DownloadState.FinalUrlNotFound => "无法获取最终的直链地址。",
        _ => $"未知状态: {state}"
    };

    /// <summary>
    /// 格式化文件大小
    /// </summary>
    public static string FormatFileSize(long bytes)
    {
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }
}