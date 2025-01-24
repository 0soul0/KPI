namespace ReviewWebsite.Helpers
{
    public class IdGenerator
    {
        private static Random _random = new Random();

        public static string GenerateUnitId(int length = 15)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var id = new char[length];
            for (int i = 0; i < id.Length; i++)
            {
                id[i] = chars[_random.Next(chars.Length)];
            }
            return new string(id);
        }
    }
}
