using System.Linq;

namespace do9Rename.Core
{
    class SubstractOperation : RenameOperation
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool HeadFirst { get; set; }
        
        public override string Execute(string input)
        {
            string newStr = HeadFirst ?
                new string(input
                            .Skip(this.Skip)
                            .Take(this.Take)
                            .ToArray()) :
                new string(input
                            .Reverse()
                            .Skip(this.Skip)
                            .Take(this.Take)
                            .Reverse()
                            .ToArray());
            return newStr;
        }
    }
}
