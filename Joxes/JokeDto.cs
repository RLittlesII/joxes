namespace Joxes;

public class JokeDto
{
    public bool error { get; set; }
    public string category { get; set; }
    public string type { get; set; }
    public string setup { get; set; }
    public string delivery { get; set; }
    public IgnoreType flags { get; set; }
    public int id { get; set; }
    public bool safe { get; set; }
    public string lang { get; set; }
}