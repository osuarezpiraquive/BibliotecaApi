using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BibliotecaApi.Services
{
    public class LogBackgroundService : BackgroundService
    {
        private readonly ILogger<LogBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _logFolder;
        private readonly string _processedFolder;

        public LogBackgroundService(ILogger<LogBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _logFolder = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            _processedFolder = Path.Combine(_logFolder, "Processed");

            if (!Directory.Exists(_logFolder))
                Directory.CreateDirectory(_logFolder);

            if (!Directory.Exists(_processedFolder))
                Directory.CreateDirectory(_processedFolder);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("LogBackgroundService iniciado.");

            // Crear un archivo de log automático al iniciar (solo para pruebas)
            await CreateSampleLogFile();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var files = Directory.GetFiles(_logFolder, "*.log");

                    foreach (var file in files)
                    {
                        await ProcessLogFile(file);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error procesando archivos de log.");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task CreateSampleLogFile()
        {
            var sampleLogPath = Path.Combine(_logFolder, $"auto_{DateTime.Now:yyyyMMdd_HHmmss}.log");

            var sampleLines = new[]
            {
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [INFO] User login: autoUser",
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [INFO] Memory usage at 65%",
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [ERROR] Database error simulated"
            };

            await File.WriteAllLinesAsync(sampleLogPath, sampleLines);
        }

        private async Task ProcessLogFile(string filePath)
        {
            _logger.LogInformation($"Procesando archivo: {Path.GetFileName(filePath)}");

            var lines = await File.ReadAllLinesAsync(filePath);

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                foreach (var line in lines)
                {
                    var logMetric = ParseLogLine(line);
                    if (logMetric != null)
                    {
                        context.LogMetrics.Add(logMetric);
                    }
                }

                await context.SaveChangesAsync();
            }

            var destination = Path.Combine(_processedFolder, Path.GetFileName(filePath));
            if (File.Exists(destination))
                File.Delete(destination);

            File.Move(filePath, destination);

            _logger.LogInformation($"Archivo procesado y movido a {destination}");
        }

        private LogMetric? ParseLogLine(string line)
        {
            var regex = new Regex(@"^(?<timestamp>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}) \[(?<level>[A-Z]+)\] (?<message>.+)$");

            var match = regex.Match(line);
            if (!match.Success) return null;

            var timestamp = DateTime.Parse(match.Groups["timestamp"].Value);
            var level = match.Groups["level"].Value;
            var message = match.Groups["message"].Value;

            string? metricType = null;
            string? metricValue = null;

            if (message.Contains("login", StringComparison.OrdinalIgnoreCase))
            {
                metricType = "UserLogin";
                metricValue = message.Split(':').Length > 1 ? message.Split(':')[1].Trim() : null;
            }
            else if (message.Contains("memory", StringComparison.OrdinalIgnoreCase))
            {
                metricType = "MemoryUsage";
                metricValue = Regex.Match(message, @"\d+%").Value;
            }
            else if (message.Contains("Database", StringComparison.OrdinalIgnoreCase))
            {
                metricType = "DatabaseError";
                metricValue = message;
            }

            return new LogMetric
            {
                Timestamp = timestamp,
                Level = level,
                Message = message,
                MetricType = metricType,
                MetricValue = metricValue
            };
        }
    }
}
