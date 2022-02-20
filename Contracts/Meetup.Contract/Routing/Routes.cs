﻿namespace Meetup.Contract.Routing;

public static class Routes
{
    public static class Auth
    {
        public const string AuthenticateUser = "auth/authenticate";
        public const string ChangeCredentials = "auth/credentials";
        public const string GetCurrentUserInfo = "auth/who-am-i";
        public const string RefreshTokenPair = "auth/refresh";
        public const string RegisterNewUser = "auth/register";
        public const string SignOutEverywhere = "auth/sign-out-everywhere";
    }

    public static class Feed
    {
        public const string GetMeetups = "feed";
        public const string GetSignedUpGuestsInfo = "feed/{meetupId:guid}/signed-up-guests";
        public const string GetSpecificMeetup = "feed/{meetupId:guid}";
        public const string SignUpForMeetup = "feed/{meetupId:guid}/sign-up";
    }

    public static class Studio
    {
        public const string DeleteSpecificMeetup = "studio/{meetupId:guid}";
        public const string GetOrganizedMeetups = "studio/organized";
        public const string RegisterNewMeetup = "studio/new";
        public const string UpdateSpecificMeetup = "studio/{meetupId:guid}";
    }
}