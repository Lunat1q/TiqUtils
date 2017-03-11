using System.Text.RegularExpressions;

namespace TiqUtils.Events.Controls
{
    public class ControlsInput
    {
        private static bool IsTextAllowed(string text)
        {
            var regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        public static bool OnlyNum(char ch)
        {
            if (!char.IsControl(ch)
                && !char.IsDigit(ch))
            {
                return true;
            }
            return ch == ',' || ch == '.';
        }
    }
}
