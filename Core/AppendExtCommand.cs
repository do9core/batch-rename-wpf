namespace do9Rename.Core
{
    internal class AppendExtCommand : IAppendExtCommand
    {
        public string Extension { get; set; }

        public string Execute(string input)
        {
            return input.EndsWith(Extension) ? input : input + Extension;
        }
    }
}
