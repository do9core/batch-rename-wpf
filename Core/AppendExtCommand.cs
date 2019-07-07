namespace do9Rename.Core
{
    internal class AppendExtCommand : IModifyExtCommand
    {
        public string Extension { get; set; }

        public string Execute(string input)
        {
            return input.EndsWith(Extension) ? input : input + Extension;
        }

        public string ToString(bool isDisplayText = false)
        {
            return isDisplayText ? $"添加扩展名{Extension}" : $"AppendExtCommand[Extension({Extension})]";
        }
    }
}
