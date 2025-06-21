public class TaskDto
{
    public required string? Title { get; set; }
    public required string? Description { get; set; }
    public required DateTime Deadline { get; set; }
    public required bool IsCompleted { get; set; } = false;

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

public class UpdateTaskDto
{
    public required int Id { get; set; }
    public required string? Title { get; set; }
    public required string? Description { get; set; }
    public required DateTime Deadline { get; set; }
    public required bool IsCompleted { get; set; } = false;

    public bool IsPriority { get; set; }


}
