namespace KCNLanzouDirectLink.Command.Commands;

/// <summary>
/// 命令接口
/// </summary>
/// <typeparam name="TOptions">命令选项类型</typeparam>
public interface ICommand<in TOptions>
{
    Task<int> ExecuteAsync(TOptions options);
}
