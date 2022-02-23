namespace Meetups.Backend.Domain.Entities.Meetup;

using System;
using Meetups.Backend.Domain.Seedwork;

public class City
{
    #region Validation

    private static void EnsureValidId(Guid id) =>
        Assertions.EnsureValidGuid(nameof(id), id, required: true);

    private static void EnsureValidName(string name) =>
        Assertions.EnsureValidString(nameof(name), name, required: true, maxLength: 30);
    
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