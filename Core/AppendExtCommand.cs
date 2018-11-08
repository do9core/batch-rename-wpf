namespace do9Rename.Core
{
    internal class AppendExtCommand : RenameCommand
    {
        public string Extension { get; set; }

        public override string Execute(string input)
        {
            return input.EndsWith(Extension) ? input : input + Extension;
        }
    }
}
