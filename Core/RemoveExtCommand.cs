namespace do9Rename.Core
{
    internal class RemoveExtCommand : IModifyExtCommand
    {
        public string Extension { get; set; }

        public string Execute(string input)
        {
            return !input.EndsWith(Extension) ?
                input :
                input.Remove(input.Length - Extension.Length, Extension.Length);
        }

        public string ToString(bool isDisplayText = false)
        {
            return isDisplayText ? $"移除扩展名{Extension}" : $"RemoveExtCommand[Extension({Extension})]";
        }
    }
}
