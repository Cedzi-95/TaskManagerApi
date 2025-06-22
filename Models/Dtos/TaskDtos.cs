public class CreateTaskDto
{
    public required string? Title { get; set; }
    public required string? Description { get; set; }
    public required DateTime Deadline { get; set; }
    public required bool IsCompleted { get; set; }
    public bool IsPriority { get; set; }
    public required string? UserId { get; set; }
    


}

public class CreateTaskResponse
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsPriority { get; set; }
    public string? CreatedBy { get; set; }
}

public class UpdateTaskDto
{
    public required int Id { get; set; }
    public required string? Title { get; set; }
    public required string? Description { get; set; }
    public required DateTime Deadline { get; set; }
    public required bool IsCompleted { get; set; } = false;
    public bool IsPriority { get; set; }
    public required string? UserId { get; set; }
}

public class UpdateTaskResponse
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsPriority { get; set; }
    public string? UpdatedBy { get; set; }
}

public class CompleteTaskDto
{
    public required int Id { get; set; }
    public string? UserId { get; set; }

}
public class CompleteTaskResponse
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsPriority { get; set; }
    public string? CompletedBy { get; set; }
}


