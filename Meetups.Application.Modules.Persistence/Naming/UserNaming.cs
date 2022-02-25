namespace Meetups.Application.Modules.Persistence.Naming;

internal static class UserNaming
{
    public const string Table = "users";

    public static class Columns
    {
        public const string Id = "id";
        public const string Username = "username";
        public const string Password = "password";
        public const string DisplayName = "display_name";
        public const string Discriminator = "role";
    }

    public static class Indices
    {
        public const string PrimaryKey = "pk_users";
        public const string UniqueUsername = "ux_users_username";
    }
}