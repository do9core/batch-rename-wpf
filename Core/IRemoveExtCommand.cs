namespace do9Rename.Core
{
    public interface IRemoveExtCommand : IRenameCommand
    {
        string Extension { get; set; }
    }
}
