public class TaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsCompleted { get; set; }

    public bool IsPriority { get; set; }


}

public class TaskResponse
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsPriority { get; set; }
}

