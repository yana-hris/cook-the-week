namespace CookTheWeek.Web.Infrastructure.Extensions
{
    using System;


    /// <summary>
    /// Provides extension methods for converting and validating GUID strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the specified GUID string to its <see cref="Guid"/> equivalent.
        /// Returns the parsed <see cref="Guid"/> if the conversion succeeded; otherwise, returns the specified default value.
        /// </summary>
        /// <param name="guidString">The string representing a GUID to convert.</param>
        /// <param name="defaultValue">The default value to return if the conversion fails. Defaults to <see cref="Guid.Empty"/>.</param>
        public static Guid ToGuidOrDefault(this string guidString, Guid defaultValue = default(Guid))
        {
            if (Guid.TryParse(guidString, out var parsedGuid))
            {
                return parsedGuid;
            }

            return defaultValue; // Return the default value if parsing fails
        }

        /// <summary>
        /// Attempts to parse a string into a Guid.
        /// Returns a boolean indicating whether the parsing was successful.
        /// </summary>
        /// <param name="guidString">The string representation of the Guid to parse.</param>
        /// <param name="parsedGuid">The parsed Guid, or Guid.Empty if parsing fails.</param>
        /// <returns>True if the string was successfully parsed into a Guid; otherwise, false.</returns>

        public static bool TryToGuid(this string guidString, out Guid parsedGuid)
        {
            return Guid.TryParse(guidString, out parsedGuid);
        }

        /// <summary>
        /// Determines whether a given string is a valid Guid.
        /// </summary>
        /// <param name="guidString">The string to check.</param>
        /// <returns>True if the string is a valid Guid; otherwise, false.</returns>

        public static bool IsValidGuid(this string guidString)
        {
            return Guid.TryParse(guidString, out _);
        }
    }

}
