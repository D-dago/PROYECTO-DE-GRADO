namespace OrientacionVocacional.Domain.Entities;

public sealed class Question
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string Text { get; set; }
    public required string Dimension { get; set; }
    public int Order { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Answer> Answers { get; } = [];
}