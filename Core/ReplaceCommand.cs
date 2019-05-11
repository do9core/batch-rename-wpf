using System.Text.RegularExpressions;

namespace do9Rename.Core
{
    internal class ReplaceCommand : IRenameCommand
    {
        public string OldText { get; set; }
        public string NewText { get; set; }
        public bool IsUsingRegex { get; set; } = false;

        public string Execute(string input)
        {
            return IsUsingRegex ?
                Regex.Replace(input, OldText, NewText) :
                input.Replace(OldText, NewText);
        }

        public override string ToString()
        {
            return IsUsingRegex ?
                $"替换正则内容为" + (NewText == string.Empty ? "[空内容]" : NewText) :
                $"替换{OldText}为" + (NewText == string.Empty ? "[空内容]" : NewText);
        }
    }
}
