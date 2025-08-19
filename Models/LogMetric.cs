namespace BibliotecaApi.Models
{
public class LogMetric
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? MetricType { get; set; }
    public string? MetricValue { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
}