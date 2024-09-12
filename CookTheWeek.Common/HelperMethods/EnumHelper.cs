namespace CookTheWeek.Common.HelperMethods
{
    public static class EnumHelper
    {
        public static IDictionary<int, string> GetEnumValuesDictionary<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .ToDictionary(
                    value => Convert.ToInt32(value),
                    value => value.ToString()
                );
        }
    }
}
