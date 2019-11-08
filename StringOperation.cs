using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace đồ_án_1___interface
{
   
    public class StringArgs
    {
    }

    public abstract class StringOperation
    {
        public StringArgs Args { get; set; }
        public abstract string Operate(string origin);
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract StringOperation Clone();

        //public abstract void Config();
    }

    public class ReplaceArgs : StringArgs
    {
        public string From { get; set; }
        public string To { get; set; }

    }

    public class ReplaceOperation : StringOperation
    {
        public override string Operate(string origin)
        {
            var args = Args as ReplaceArgs;
            var from = args.From;
            var to = args.To;
            return origin.Replace(from, to);
        }

        public override StringOperation Clone()
        {
            var oldArgs = Args as ReplaceArgs;
            return new ReplaceOperation()
            {
                Args = new ReplaceArgs()
                {
                    From = oldArgs.From,
                    To = oldArgs.To
                }
            };
        }

        //public override void Config()
        //{
        //    var screen = new ReplaceConfigDialog(Args);
        //    if (screen.ShowDialog() == true)
        //    {

        //    }
        //}

        public override string Name => "Replace";
        public override string Description
        {
            get
            {
                var args = Args as ReplaceArgs;
                return $"Replace from {args.From} to {args.To}";
            }
        }
    }

    public class NewCaseUpperOperation : StringOperation
    {
        public override string Name => "Upper case";

        public override string Description
        {
            get
            {
                return "Upper case all";
            }
        }


        public override StringOperation Clone()
        {
            return new NewCaseUpperOperation();
        }

        public override string Operate(string origin)
        {
            return origin.ToUpper();
        }
    }

    public class NewCaseLowerOperation : StringOperation
    {
        public override string Name => "Lower case";

        public override string Description
        {
            get
            {
                return "Lower case all";
            }
        }

        public override StringOperation Clone()
        {
            return new NewCaseLowerOperation();
        }

        public override string Operate(string origin)
        {
            return origin.ToLower();
        }
    }

    public class NewCaseFirst : StringOperation
    {
        public override string Name => "First letter";

        public override string Description 
        {
            get
            {
                return "Upper case all first letter";
            }
        }

        public override StringOperation Clone()
        {
            return new NewCaseFirst();
        }

        public override string Operate(string origin)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(origin.ToLower());
        }
    }

    public class Normalize : StringOperation
    {
        public override string Name => "Normalize";

        public override string Description
        {
            get
            {
                return "Upper case first letter, trim whitespace";
            }
        }

        public override StringOperation Clone()
        {
            return new Normalize();
        }

        public override string Operate(string origin)
        {
            origin = origin.Trim();
            origin = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(origin.ToLower());

            string result = "";

            for (int i = 0; i < origin.Length; i++)
            {
                if (!(origin[i] == 32 && origin[i+1] == 32))
                {
                    result += origin[i];
                }
            }

            return result;
        }
    }

    public class MoveToEnd : StringOperation
    {
        public override string Name => "Move to end";

        public override string Description
        {
            get
            {
                return "Move 13 character to the end";
            }
        }

        public override StringOperation Clone()
        {
            return new MoveToEnd();
        }

        public override string Operate(string origin)
        {
            string result = "";
            result += origin.Substring(13);
            result += " ";
            result += origin.Substring(0, 12);
            return result;
        }
    }
}
