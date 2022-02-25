namespace Meetups.Application.Modules.Persistence;

public interface IPersistenceConfiguration
{
    string ConnectionString { get; }
}