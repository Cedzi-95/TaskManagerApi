export class Task {
    constructor(id, title, description, createdAt, deadline, IsCompleted, IsPriority, userId, user){
        this.id = id;
        this.title = title;
        this.description = description;
        this.createdAt = new Date(Date.parse(createdAt));
        this.deadline = deadline;
        this.IsCompleted = IsCompleted;
        this.IsPriority = IsPriority;
        this.userId = userId;
        this.User = user;
    }

}
/*public required int Id { get; set; }
    public required string? Title { get; set; }
    public required string? Description { get; set; }
    public required DateTime CreateAt { get; set; }
    public required DateTime Deadline { get; set; }
    public required bool IsCompleted { get; set; }
    public bool IsPriority { get; set; }
        [ForeignKey("userId")]
    public string? UserId { get; set; }  
    public UserEntity? User { get; set; } */