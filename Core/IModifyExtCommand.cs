namespace do9Rename.Core
{
    public interface IModifyExtCommand : IRenameCommand
    {
        string Extension { get; set; }
    }
}
