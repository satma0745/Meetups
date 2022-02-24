namespace Meetups.Backend.Application.Modules.Persistence.Naming;

internal static class RefreshTokenNaming
{
    public const string Table = "refresh_tokens";

    public static class Columns
    {
        public const string TokenId = "token_id";
        public const string BearerId = "user_id";
    }

    public static class Indices
    {
        public const string PrimaryKey = "pk_refresh_tokens";
        public const string BearerId = "ix_refresh_tokens_user_id";
    }

    public static class ForeignKeys
    {
        public const string BearerId = "fk_users_refresh_tokens_user_id";
    }
}