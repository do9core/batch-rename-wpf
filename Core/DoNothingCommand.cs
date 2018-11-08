namespace do9Rename.Core
{
    internal class DoNothingCommand : RenameCommand
    {
        public override string Execute(string input)
        {
            return input;
        }
    }
}
