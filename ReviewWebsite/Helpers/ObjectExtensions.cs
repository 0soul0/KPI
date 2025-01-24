namespace ReviewWebsite.Helpers
{
    public static class ObjectExtensions
    {
        public static string GetValueOrDefault(this string value)
        {
            return string.IsNullOrEmpty(value) ? "" : value;
        }

    }
}
