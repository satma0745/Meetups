namespace Meetups.Persistence.Naming;

internal static class MeetupsGuestsSignupSkipNaming
{
    public const string Table = "meetups_guests_signup";

    public static class Columns
    {
        public const string MeetupId = "meetup_id";
        public const string GuestId = "signed_up_guest_id";
    }

    public static class Indices
    {
        public const string PrimaryKey = "pk_meetups_guests_signup";
        public const string GuestId = "ix_meetups_guests_signup_signed_up_guest_id";
    }

    public static class ForeignKeys
    {
        public const string MeetupId = "fk_meetups_guests_signup_meetups_meetup_id";
        public const string GuestId = "fk_meetups_guests_signup_guests_signed_up_guest_id";
    }
}