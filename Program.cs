using CommandLine;
using KCNLanzouDirectLink.Command.Commands;
using KCNLanzouDirectLink.Command.Options;
using KCNLanzouDirectLink.Command.Services;

namespace KCNLanzouDirectLink.Command;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var outputService = new ConsoleOutputService();

        var result = await Parser.Default.ParseArguments<GetLinkOptions, GetInfoOptions, BatchOptions>(args)
            .MapResult(
                async (GetLinkOptions opts) => await new GetLinkCommand(outputService).ExecuteAsync(opts),
                async (GetInfoOptions opts) => await new GetInfoCommand(outputService).ExecuteAsync(opts),
                async (BatchOptions opts) => await new BatchCommand(outputService).ExecuteAsync(opts),
                errs => Task.FromResult(1)
            );

        return result;
    }
}