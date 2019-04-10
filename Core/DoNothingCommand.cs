namespace do9Rename.Core
{
    internal class DoNothingCommand : IRenameCommand
    {
        public string Execute(string input)
        {
            return input;
        }
    }
}
