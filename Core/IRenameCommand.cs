namespace do9Rename.Core
{
    public interface IRenameCommand
    {
        string Execute(string input);

        string ToString(bool isDisplayText = false);
    }
}
