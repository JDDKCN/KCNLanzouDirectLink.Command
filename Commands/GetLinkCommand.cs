using KCNLanzouDirectLink.Command.Options;
using KCNLanzouDirectLink.Command.Services;
using KCNLanzouDirectLink.Command.Utils;
using KCNLanzouDirectLink.Models;

namespace KCNLanzouDirectLink.Command.Commands;

/// <summary>
/// 获取直链命令
/// </summary>
public class GetLinkCommand : ICommand<GetLinkOptions>
{
    private readonly IOutputService _output;

    public GetLinkCommand(IOutputService outputService)
    {
        _output = outputService ?? throw new ArgumentNullException(nameof(outputService));
    }

    public async Task<int> ExecuteAsync(GetLinkOptions options)
    {
        try
        {
            if (!options.Quiet && !options.JsonOutput)
            {
                _output.WriteInfo($"正在解析链接: {options.Url}");
            }

            var (state, url) = string.IsNullOrWhiteSpace(options.Password)
                ? await KCNLanzouLinkHelper.GetDirectLinkAsync(options.Url)
                : await KCNLanzouLinkHelper.GetDirectLinkAsync(options.Url, options.Password, options.RetryCount);

            if (options.JsonOutput)
            {
                _output.WriteJson(new
                {
                    Success = state == DownloadState.Success,
                    State = state.ToString(),
                    StateDescription = ResultFormatter.GetStateDescription(state),
                    SourceUrl = options.Url,
                    DirectLink = url
                });
                return state == DownloadState.Success ? 0 : 1;
            }

            if (options.Quiet)
            {
                if (state == DownloadState.Success && !string.IsNullOrEmpty(url))
                {
                    _output.WriteLine(url);
                }
                return state == DownloadState.Success ? 0 : 1;
            }

            if (state == DownloadState.Success && !string.IsNullOrEmpty(url))
            {
                _output.WriteSuccess("解析成功!");
                _output.WriteLine();
                _output.WriteLine($"  源链接: {options.Url}");
                _output.WriteLine($"  直链:   {url}");
                return 0;
            }
            else
            {
                _output.WriteError($"解析失败: {ResultFormatter.GetStateDescription(state)}");
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
                    DirectLink = (string?)null
                });
            }
            else if (!options.Quiet)
            {
                _output.WriteError($"发生异常: {ex.Message}");
            }
            return 1;
        }
    }
}