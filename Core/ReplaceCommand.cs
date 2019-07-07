using System.Text.RegularExpressions;

namespace do9Rename.Core
{
    internal class ReplaceCommand : IRenameCommand
    {
        public string OldText { get; }
        public string NewText { get; private set; }
        public bool IsUsingRegex { get; }

        public ReplaceCommand(string oldText, string newText, bool isUsingRegex)
        {
            this.OldText = oldText;
            this.NewText = newText;
            this.IsUsingRegex = isUsingRegex;
        }

        public string Execute(string input)
        {
            NewText = string.IsNullOrWhiteSpace(NewText) ? string.Empty : NewText;
            return IsUsingRegex ?
                Regex.Replace(input, OldText, NewText) :
                input.Replace(OldText, NewText);
        }

        public override string ToString()
        {
            var newText = string.IsNullOrWhiteSpace(NewText) ? "[空内容]" : NewText;
            return IsUsingRegex ?
                $"替换正则内容为{newText}" :
                $"替换{OldText}为{newText}";
        }

        public string ToString(bool isDisplayText = false)
        {
            return isDisplayText ? base.ToString() :
                $"ReplaceCommand[OldText({OldText}), NewText({NewText}), IsUsingRegex({IsUsingRegex})]";
        }
    }
}
