namespace Meetups.Application.Modules.Persistence.Naming;

internal static class MeetupNaming
{
    public const string Table = "meetups";

    public static class Columns
    {
        public const string Id = "id";
        public const string Topic = "topic";
        public const string CityId = "place_city_id";
        public const string Address = "place_address";
        public const string Duration = "duration";
        public const string StartTime = "start_time";
        public const string OrganizerId = "organizer_id";
    }

    public static class Indices
    {
        public const string PrimaryKey = "pk_meetups";
        public const string UniqueTopic = "ux_meetups_topic";
        public const string OrganizerId = "ix_meetups_organizer_id";
        public const string CityId = "ix_meetups_place_city_id";
    }

    public static class ForeignKeys
    {
        public const string CityId = "fk_meetups_cities_place_city_id";
        public const string OrganizerId = "fk_meetups_organizers_organizer_id";
    }
}