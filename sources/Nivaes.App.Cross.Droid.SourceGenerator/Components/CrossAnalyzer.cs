namespace Nivaes.App.Cross
{
    public static class CrossAnalyzer
    {
        public static string Version { get; } = typeof(CrossAnalyzer).Assembly.GetName().Version.ToString();
        public static string CodeGenerationAttribute { get; } = $@"[System.CodeDom.Compiler.GeneratedCode(""Nivaes.App.Cross"", ""{Version}"")]";
    }
}
