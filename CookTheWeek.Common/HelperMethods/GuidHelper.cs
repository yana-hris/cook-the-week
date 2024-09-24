namespace CookTheWeek.Common.HelperMethods
{
    using System;
    public static class GuidHelper
    {
        public static bool CompareGuidStrings(string guidString1, string guidString2)
        {
            if (Guid.TryParse(guidString1, out Guid guid1) && Guid.TryParse(guidString2, out Guid guid2))
            {
                return guid1 == guid2;
            }
            else
            {
                throw new ArgumentException("One or both strings are not valid GUIDs.");
            }
        }
    }
}
