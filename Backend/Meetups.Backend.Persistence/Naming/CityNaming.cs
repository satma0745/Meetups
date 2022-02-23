namespace Meetups.Backend.Persistence.Naming;

internal static class CityNaming
{
    public const string Table = "cities";

    public static class Columns
    {
        public const string Id = "id";
        public const string Name = "name";
    }

    public static class Indices
    {
        public const string PrimaryKey = "pk_cities";
        public const string UniqueName = "ux_cities_name";
    }
}