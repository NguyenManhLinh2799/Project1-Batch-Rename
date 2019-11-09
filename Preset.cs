using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace đồ_án_1___interface
{
    public class JSONOperation
    {
        public string OperationName { get; set; }
        public List<string> Args { get; set; }
        public int IntArgs { get; set; }
    }

    public class Preset
    {
        public string PresetName { get; set; }
        public List<JSONOperation> List { get; set; }

        // Convert from StringOperation to JSONOperation, add to Preset List to store
        public void AddToList(StringOperation method)
        {
            if (method is ReplaceOperation)
            {
                var temp = new JSONOperation() { Args = new List<string>() };
                temp.OperationName = method.Name;
                var args = (ReplaceArgs) method.Args as ReplaceArgs;
                temp.Args.Add(args.From);
                temp.Args.Add(args.To);
                List.Add(temp);
            }
            else if (method is NewCaseOperation)
            {
                var temp = new JSONOperation();
                temp.OperationName = method.Name;
                var args = (NewCaseArgs)method.Args as NewCaseArgs;
                temp.IntArgs = args.Mode;
                List.Add(temp);
            }
            else if (method is MoveOperation)
            {
                var temp = new JSONOperation();
                temp.OperationName = method.Name;
                var args = (MoveArgs)method.Args as MoveArgs;
                temp.IntArgs = args.Mode;
                List.Add(temp);
            }
            else
            {
                var temp = new JSONOperation();
                temp.OperationName = method.Name;
                List.Add(temp);
            }
        }
    }

    public class PresetList
    {
        public int Count { get; set; } = 0;
        public List<Preset> List { get; set; }
        public void AddToList(Preset a)
        {
            List.Add(a);
            Count++;
        }
    }

}
