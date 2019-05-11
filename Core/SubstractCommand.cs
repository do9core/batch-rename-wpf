using System.Linq;

namespace do9Rename.Core
{
    internal class SubstractCommand : IRenameCommand
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool HeadFirst { get; set; }

        public string Execute(string input)
        {
            return HeadFirst ?
                new string(input
                            .Skip(Skip)
                            .Take(Take)
                            .ToArray()) :
                new string(input
                            .Reverse()
                            .Skip(Skip)
                            .Take(Take)
                            .Reverse()
                            .ToArray());
        }

        public override string ToString()
        {
            return "从" + (HeadFirst ? "头" : "尾") + $"截取第{Skip}到第{Take}位字符";
        }
    }
}
