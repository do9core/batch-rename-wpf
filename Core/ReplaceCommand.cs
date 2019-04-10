using System.Text.RegularExpressions;

namespace do9Rename.Core
{
    internal class ReplaceCommand : IRenameCommand
    {
        private const string RegexPrefix = "#regex#:";

        public string OldText { get; set; }
        public string NewText { get; set; }

        public string Execute(string input)
        {
            if (!OldText.StartsWith(RegexPrefix)) return input.Replace(OldText, NewText);
            var raw = OldText.Remove(0, RegexPrefix.Length).Trim();

            return Regex.Replace(input, raw, NewText);
        }
    }
}
