namespace Meetups.Backend.Entities.Meetup;

using System;

public class City
{
    #region Validation

    private static void EnsureValidId(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Must not be empty.", nameof(id));
        }
    }

    private static void EnsureValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Must not be null or empty.", nameof(name));
        }

        const int maxLength = 30;
        if (name.Length > maxLength)
        {
            throw new ArgumentException($"Must not exceed {maxLength} characters.", nameof(name));
        }
    }
    
    #endregion
    
    #region State
    
    public Guid Id { get; }
    
    public string Name { get; }
    
    #endregion
    
    #region Constructors

    public City(string name)
        : this(id: Guid.NewGuid(), name)
    {
    }
    
    private City(Guid id, string name)
    {
        EnsureValidId(id);
        EnsureValidName(name);

        Id = id;
        Name = name;
    }
    
    #endregion

    #region Equality

    public override int GetHashCode() =>
        Id.GetHashCode();

    public override bool Equals(object obj) =>
        ReferenceEquals(obj, this) ||
        obj is City other &&
        other.Id == Id &&
        other.Name == Name;

    #endregion
}