namespace Joxes.Blazor.Pages.Chuck;

public class DeliveredJokeViewModel
{
    public DeliveredJokeViewModel(JokeResponse jokeResponse)
    {
        Id = jokeResponse.Id.Value;
        UserId = jokeResponse.UserId.Value;
        Punchline = jokeResponse.JokeDto.Value;
        Category = string.Join(", ", jokeResponse.JokeDto.Categories);
        DelvieredAt = jokeResponse.Timestamp;
    }

    public string Id { get; set; }
    public string UserId { get; set; }
    public string Category { get; set; }
    public string Punchline { get; set; }
    public DateTimeOffset DelvieredAt { get; set; }
}