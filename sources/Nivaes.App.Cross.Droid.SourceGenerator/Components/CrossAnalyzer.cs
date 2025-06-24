namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class CrossAnalyzer
    {
        public static string Version { get; } = typeof(CrossAnalyzer).Assembly.GetName().Version.ToString();
        public static string CodeGenerationAttribute { get; } = $@"[System.CodeDom.Compiler.GeneratedCode(""Nivaes.App.Cross"", ""{Version}"")]";
    }
}
