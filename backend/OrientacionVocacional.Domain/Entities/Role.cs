namespace OrientacionVocacional.Domain.Entities;

public sealed class Role
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<User> Users { get; } = [];
}