namespace Meetups.Backend.Application.Modules.Persistence;

public interface IPersistenceConfiguration
{
    string ConnectionString { get; }
}