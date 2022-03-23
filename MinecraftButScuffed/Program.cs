using System;
using Serilog;

namespace MinecraftButScuffed;

public static class Program
{
    [STAThread]
    private static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

        using var game = new MinecraftButScuffed();
        game.Run();
    }
}