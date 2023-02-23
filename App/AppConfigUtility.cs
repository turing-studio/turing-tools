using System;
using System.Collections.Generic;

namespace Turing.Tools.App
{
    public static class AppConfigUtility
    {
        public static bool IsHeadless => 
            Environment.CommandLine.Contains("-batchmode") && Environment.CommandLine.Contains("-nographics");

        public static string GetArgument(params string[] names)
        {
            var nameSet = new HashSet<string>(names);
            var args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length - 1; i++)
                if (nameSet.Contains(args[i]))
                    return args[i + 1];
            return null;
        }
    }
}