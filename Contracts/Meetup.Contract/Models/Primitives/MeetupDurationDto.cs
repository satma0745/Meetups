namespace Meetup.Contract.Models.Primitives;

using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

[JsonConverter(typeof(Converter))]
public class MeetupDurationDto
{
    public int Hours { get; set; }
    
    public int Minutes { get; set; }

    public MeetupDurationDto(int hours, int minutes)
    {
        Hours = hours;
        Minutes = minutes;
    }

    private class Converter : JsonConverter<MeetupDurationDto>
    {
        private const string HoursGroupName = "Hours";
        private const string MinutesGroupName = "Minutes";
        private const string Pattern = $@"^(?<{HoursGroupName}>\d+):(?<{MinutesGroupName}>\d{{2}})$";
        private static readonly Regex Regex = new(Pattern, RegexOptions.Compiled);

        private const string UnsupportedTypeErrorMessage = "Only string tokens are supported.";
        private const string InvalidFormatErrorMessage = "Value must be in the \"H:mm\" format.";
        private const string InvalidMinutesCountErrorMessage = "Invalid number of minutes.";

        private const string SerializedValueTemplate = "{0}:{1:00}";

        public override MeetupDurationDto Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException(UnsupportedTypeErrorMessage);
            }

            var source = reader.GetString()!;
            var match = Regex.Matches(source).SingleOrDefault();
            if (match is null)
            {
                throw new JsonException(InvalidFormatErrorMessage);
            }

            var hours = int.Parse(match.Groups[HoursGroupName].Value);
            var minutes = int.Parse(match.Groups[MinutesGroupName].Value);
            if (minutes >= 60)
            {
                throw new JsonException(InvalidMinutesCountErrorMessage);
            }

            return new MeetupDurationDto(hours, minutes);
        }

        public override void Write(Utf8JsonWriter writer, MeetupDurationDto duration, JsonSerializerOptions options)
        {
            var serialized = string.Format(SerializedValueTemplate, duration.Hours, duration.Minutes);
            writer.WriteStringValue(serialized);
        }
    }
}