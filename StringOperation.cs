using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace đồ_án_1___interface
{

    // ===============================================================================
    // Parent classes ================================================================
    // ===============================================================================
    public class StringArgs
    {
    }

    public abstract class StringOperation
    {
        public StringArgs Args { get; set; }
        public abstract string Operate(string origin);
        public abstract StringOperation Clone();
        public abstract void Config();
        public abstract string Name { get; }
        public abstract string Description { get; }
    }

    // ===============================================================================
    // Replace Method ================================================================
    // ===============================================================================
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

        public override void Config()
        {
            var screen = new ReplaceConfigDialog(Args);
            if (screen.ShowDialog() == true)
            {

            }
        }

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

    // ===============================================================================
    // New Case Method ===============================================================
    // ===============================================================================
    public class NewCaseArgs : StringArgs
    {
        // Decided by user configuration
        // 0: Uppercase all, 1: Lowercase all, 2: Uppercase first letter of each word
        public int Mode { get; set; }
    }

    public class NewCaseOperation : StringOperation
    {
        public override string Operate(string origin)
        {
            var args = Args as NewCaseArgs;
            if (args.Mode == 0)
            {
                return origin.ToUpper();
            }
            else if (args.Mode == 1)
            {
                return origin.ToLower();
            }
            else if (args.Mode == 2)
            {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(origin.ToLower());
            }
            else
            {
                return "";
            }
        }

        public override StringOperation Clone()
        {
            var oldArgs = Args as NewCaseArgs;
            return new NewCaseOperation()
            {
                Args = new NewCaseArgs()
                {
                    Mode = oldArgs.Mode
                }
            };
        }

        public override void Config()
        {
            var screen = new NewCaseConfigDialog(Args);
            if (screen.ShowDialog() == true)
            {

            }
        }

        public override string Name => "New Case";

        public override string Description
        {
            get
            {
                var args = Args as NewCaseArgs;
                if (args.Mode == 0)
                {
                    return "Uppercase all";
                }
                else if (args.Mode == 1)
                {
                    return "Lowercase all";
                }
                else if (args.Mode == 2)
                {
                    return "Uppercase first letter of each word";
                }
                else
                {
                    return "";
                }
            }
        }
    }

    // ===============================================================================
    // Fullname Normalize Method =====================================================
    // ===============================================================================

    // This method doesn't need args
    public class NormalizeOperation : StringOperation
    {
        public override string Operate(string origin)
        {
            origin = origin.Trim();
            origin = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(origin.ToLower());

            string result = "";

            // Ignore unnecessary whitespace
            for (int i = 0; i < origin.Length; i++)
            {
                if (!(origin[i] == ' ' && origin[i+1] == ' '))
                {
                    result += origin[i];
                }
            }

            return result;
        }

        public override StringOperation Clone()
        {
            return new NormalizeOperation();
        }

        public override void Config()
        {
            // No args -> no need to config
        }

        public override string Name => "Fullname Normalize";

        public override string Description => "Upper case first letter, trim whitespace";
    }

    // ===============================================================================
    // Move Method ===================================================================
    // ===============================================================================
    public class MoveArgs : StringArgs
    {
        // Decided by user configuration
        // 0: Move to the front, 1: Move to the back
        public int Mode { get; set; }
    }

    public class MoveOperation : StringOperation
    {
        public override string Operate(string origin)
        {
            string result = "";
            result += origin.Substring(13);
            result += " ";
            result += origin.Substring(0, 12);
            return result;
        }

        public override StringOperation Clone()
        {
            var oldArgs = Args as MoveArgs;
            return new MoveOperation()
            {
                Args = new MoveArgs
                {
                    Mode = oldArgs.Mode
                }
            };
        }

        public override void Config()
        {
            var screen = new MoveConfigDialog(Args);
            if (screen.ShowDialog() == true)
            {

            }
        }

        public override string Name => "Move";

        public override string Description
        {
            get
            {
                var args = Args as MoveArgs;
                return args.Mode == 0 ? "Move 13 characters to the front" : "Move 13 characters to the back";
            }
        }
    }

    // ===============================================================================
    // Unique Name Method ============================================================
    // ===============================================================================

    // This method doesn't need args
    public class UniqueNameOperation : StringOperation
    {
        public override string Operate(string origin)
        {
            return Guid.NewGuid().ToString();
        }

        public override StringOperation Clone()
        {
            return new UniqueNameOperation();
        }

        public override void Config()
        {
            // No args -> no need to config
        }

        public override string Name => "Unique Name";

        public override string Description => "Create a unique file name globally";
    }
}
