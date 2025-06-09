using System.Diagnostics;
using StackExchange.Profiling;

public class ResourceMetricsMiddleware
{
    private readonly RequestDelegate _next;

    public ResourceMetricsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var profiler = MiniProfiler.Current;
        var process = Process.GetCurrentProcess();

        process.Refresh();
        var cpuBefore = process.TotalProcessorTime;
        var ramBefore = process.PrivateMemorySize64;
        long memAllocBefore = GC.GetTotalAllocatedBytes();

        var stopwatch = Stopwatch.StartNew();

        await _next(context); // nastavi sa requestom

        stopwatch.Stop();
        process.Refresh();
        var cpuAfter = process.TotalProcessorTime;
        var ramAfter = process.PrivateMemorySize64;
        long memAllocAfter = GC.GetTotalAllocatedBytes();

        // Izračunaj razlike
        var cpuUsed = cpuAfter - cpuBefore;
        var ramUsed = ramAfter - ramBefore;
        var memAllocated = memAllocAfter - memAllocBefore;

        // Dodaj u MiniProfiler custom timing sekciju
        profiler.CustomTiming("resource", $"CPU: {cpuUsed.TotalMilliseconds} ms, RAM delta: {ramUsed / 1024} KB, MemAllocated: {memAllocated / 1024} KB, Duration: {stopwatch.ElapsedMilliseconds} ms");
    }
}