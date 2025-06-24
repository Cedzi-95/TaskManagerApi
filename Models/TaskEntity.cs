using System.ComponentModel.DataAnnotations.Schema;

public class TaskEntity
{

    public required int Id { get; set; }
    public required string? Title { get; set; }
    public required string? Description { get; set; }
    public required DateTime CreateAt { get; set; }
    public required DateTime Deadline { get; set; }
    public required bool IsCompleted { get; set; }
    public bool IsPriority { get; set; }
        [ForeignKey("userId")]
    public string? UserId { get; set; }  
    public UserEntity? User { get; set; }

}