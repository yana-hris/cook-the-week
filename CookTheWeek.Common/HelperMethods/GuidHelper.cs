namespace CookTheWeek.Common.HelperMethods
{
    using System;
    public static class GuidHelper
    {
        /// <summary>
        /// Compares two strings (intended to be GUIDS) as GUIDS. If any of the strings is not a valid GUID, returns false.
        /// </summary>
        /// <param name="guidString1"></param>
        /// <param name="guidString2"></param>
        /// <returns></returns>
        public static bool CompareTwoGuidStrings(string guidString1, string guidString2)
        {
            if (Guid.TryParse(guidString1, out Guid guid1) && Guid.TryParse(guidString2, out Guid guid2))
            {
                return guid1 == guid2;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Compares a string (GUID) with a GUID by first trying to parse the string to a valid GUID. If the string is not a valid GUID, returns false.
        /// </summary>
        /// <param name="guidString"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool CompareGuidStringWithGuid(string guidString, Guid guid)
        {
            if (Guid.TryParse(guidString, out Guid parsedGuid))
            {
                return parsedGuid == guid;
            }
            else
            {
                return false;
            }
        }
    }
}
