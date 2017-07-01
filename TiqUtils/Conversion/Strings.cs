namespace TiqUtils.Conversion
{
    public static class Strings
    {
        public static int ToInt(this string val)
        {
            int res;
            return int.TryParse(val, out res) ? res : -100;
        }
    }
}
