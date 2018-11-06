using System.Text.RegularExpressions;

namespace do9Rename.Core
{
    class ReplaceOpreation : RenameOperation
    {
        private const string REGEX_PREFIX = "#regex#:";

        public string OldText { get; set; }
        public string NewText { get; set; }

        public override string Execute(string input)
        {
            if (OldText.StartsWith(REGEX_PREFIX))
            {
                string raw = OldText.Remove(0, REGEX_PREFIX.Length).Trim();
                return Regex.Replace(input, raw, NewText);
            }

            return input.Replace(OldText, NewText);
        }
    }
}
