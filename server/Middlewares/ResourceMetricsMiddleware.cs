using System.Diagnostics;
using StackExchange.Profiling;

public class ResourceMetricsMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var profiler = MiniProfiler.Current;

        var appProcess = Process.GetCurrentProcess();
        var sqlProcess = Process.GetProcessesByName("sqlservr").FirstOrDefault();

        appProcess.Refresh();
        sqlProcess?.Refresh();

        var appCpuBefore = appProcess.TotalProcessorTime;
        var appRamBefore = appProcess.PrivateMemorySize64;
        long appAllocBefore = GC.GetTotalAllocatedBytes();

        var sqlCpuBefore = sqlProcess?.TotalProcessorTime ?? TimeSpan.Zero;
        var sqlRamBefore = sqlProcess?.PrivateMemorySize64 ?? 0;

        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        appProcess.Refresh();
        sqlProcess?.Refresh();

        var appCpuAfter = appProcess.TotalProcessorTime;
        var appRamAfter = appProcess.PrivateMemorySize64;
        long appAllocAfter = GC.GetTotalAllocatedBytes();

        var sqlCpuAfter = sqlProcess?.TotalProcessorTime ?? TimeSpan.Zero;
        var sqlRamAfter = sqlProcess?.PrivateMemorySize64 ?? 0;

        var appCpuUsed = appCpuAfter - appCpuBefore;
        var appRamDelta = appRamAfter - appRamBefore;
        var appAllocDelta = appAllocAfter - appAllocBefore;

        var sqlCpuUsed = sqlCpuAfter - sqlCpuBefore;
        var sqlRamDelta = sqlRamAfter - sqlRamBefore;

        var elapsedMs = stopwatch.ElapsedMilliseconds;

        var cpuUsed = appCpuUsed.TotalMilliseconds + sqlCpuUsed.TotalMilliseconds;
        var ramUsed = (appRamDelta + sqlRamDelta) / 1024.0;

        profiler.CustomTiming("resource", $"CPU: {cpuUsed} ms, RAM delta: {ramUsed:F2} KB, MemAllocated: {appAllocDelta / 1024} KB, Duration: {elapsedMs} ms");

        Console.WriteLine("[MiniProfiler] " + $"CPU: {cpuUsed} ms, RAM delta: {ramUsed:F2} KB, MemAllocated: {appAllocDelta / 1024} KB, Duration: {elapsedMs} ms");
    }
}