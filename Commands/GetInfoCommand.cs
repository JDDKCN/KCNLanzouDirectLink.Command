using KCNLanzouDirectLink.Command.Options;
using KCNLanzouDirectLink.Command.Services;
using KCNLanzouDirectLink.Command.Utils;
using KCNLanzouDirectLink.Models;

namespace KCNLanzouDirectLink.Command.Commands;

/// <summary>
/// 获取文件信息命令
/// </summary>
public class GetInfoCommand : ICommand<GetInfoOptions>
{
    private readonly IOutputService _output;

    public GetInfoCommand(IOutputService outputService)
    {
        _output = outputService ?? throw new ArgumentNullException(nameof(outputService));
    }

    public async Task<int> ExecuteAsync(GetInfoOptions options)
    {
        try
        {
            if (!options.JsonOutput)
            {
                _output.WriteInfo($"正在获取文件信息: {options.Url}");
            }

            var (state, fileInfo) = string.IsNullOrWhiteSpace(options.Password)
                ? await KCNLanzouLinkHelper.GetFileInfoAsync(options.Url)
                : await KCNLanzouLinkHelper.GetFileInfoAsync(options.Url, options.Password);

            if (options.JsonOutput)
            {
                _output.WriteJson(new
                {
                    Success = state == DownloadState.Success,
                    State = state.ToString(),
                    StateDescription = ResultFormatter.GetStateDescription(state),
                    SourceUrl = options.Url,
                    FileInfo = fileInfo
                });
                return state == DownloadState.Success ? 0 : 1;
            }

            if (state == DownloadState.Success && fileInfo != null)
            {
                _output.WriteSuccess("获取成功!");
                _output.WriteLine();
                _output.WriteLine("文件信息:");
                _output.WriteLine($"  文件名:   {fileInfo.FileName}");
                _output.WriteLine($"  文件大小: {fileInfo.Size}");
                _output.WriteLine($"  上传时间: {fileInfo.UploadTime}");
                _output.WriteLine($"  上传者:   {fileInfo.Uploader}");

                if (!string.IsNullOrEmpty(fileInfo.Description))
                {
                    _output.WriteLine($"  描述:     {fileInfo.Description}");
                }

                return 0;
            }
            else
            {
                _output.WriteError($"获取失败: {ResultFormatter.GetStateDescription(state)}");
                return 1;
            }
        }
        catch (Exception ex)
        {
            if (options.JsonOutput)
            {
                _output.WriteJson(new
                {
                    Success = false,
                    State = "Exception",
                    StateDescription = ex.Message,
                    SourceUrl = options.Url,
                    FileInfo = (object?)null
                });
            }
            else
            {
                _output.WriteError($"发生异常: {ex.Message}");
            }
            return 1;
        }
    }
}