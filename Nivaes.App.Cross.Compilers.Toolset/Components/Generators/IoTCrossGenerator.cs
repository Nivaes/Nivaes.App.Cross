namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;

    [Generator]
    public class IoTCrossGenerator
        : ISourceGenerator
    {
        private readonly DebuggerLog mDebuggerLog = new DebuggerLog();

        public IoTCrossGenerator()
        {
            //mDebuggerLog.DebugAppendLog("Create - ViewModelGenerator");
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //mDebuggerLog.DebugAppendLog("Initialize");

#if DEBUG
            //System.Diagnostics.Debugger.Launch();
#endif
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver(mDebuggerLog));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            //mDebuggerLog.DebugAppendLog("Execute");
#if DEBUG
            //System.Diagnostics.Debugger.Launch();
#endif     

            //mDebuggerLog.DebugAppendLog($"INI - Generating");
            try
            {
                var classes = (context.SyntaxReceiver as SyntaxReceiver)?.Classes;
                if (classes is object)
                {
                    StringBuilder sourceBuilder = new StringBuilder(@$"
                        namespace {context.Compilation.AssemblyName}.Runtime.CompilerServices
                        {{
                            using System;
                            using Nivaes.App;

                            public static class ViewModelRegisterHelper
                            {{
                                public static void RegisterViewModelTypes() 
                                {{
                                    Console.WriteLine(""Register type !"");
                        ");

                    foreach (var classSyntax in classes)
                    {
                        //Execute(context, classSyntax);
                        sourceBuilder.AppendLine($"ViewModelHelper.RegisterType(typeof({classSyntax.GetFullName()}));");
                        sourceBuilder.AppendLine($"Console.WriteLine(typeof({classSyntax.GetFullName()}).ToString());");
                    }

                    sourceBuilder.Append(@"
                            Console.WriteLine(""End register type:"");
                            }
                        }
                    }");

                    context.AddSource("Nivaes.App.Cross.Generated.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                }
            }
            catch (Exception ex)
            {
                mDebuggerLog.DebugAppendLog(ex.ToString());
            }

            //mDebuggerLog.DebugAppendLog("FIN - Generating");
        }

        //private void Execute(GeneratorExecutionContext context, ClassDeclarationSyntax classSyntax)
        //{
        //    StringBuilder sourceBuilder = new StringBuilder(@$"
        //    using System;
        //    namespace Nivaes.App.{context.Compilation.AssemblyName}.Compilers.ProtoBuf
        //    {{
        //        public static class ProtoBufHelper
        //        {{
        //            public static void AddProtoBuf() 
        //            {{
        //                Console.WriteLine(""Register type !"");

        //                //RegisterType(typeof(Nivaes.App.Test.ModelTest1));
        //                //RegisterType(typeof(Nivaes.App.Test.ModelTest2));
        //                //RegisterType(typeof(Nivaes.App.Test.ModelTest3));
        //                //RegisterType(typeof(Nivaes.App.Test.TestDataModel01));
        //    ");

        //    Compilation compilation = context.Compilation;

        //    //IEnumerable<(string, string, string)> options = GetMustacheOptions(compilation);
        //    //IEnumerable<(string, string)> namesSources = SourceFilesFromMustachePaths(options);

        //    //INamedTypeSymbol attributeSymbol = compilation.GetTypeByMetadataName("AutoNotify.AutoNotifyAttribute");
        //    //INamedTypeSymbol notifySymbol = compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");

        //    //INamedTypeSymbol protoContractSymbol = compilation.GetTypeByMetadataName("ProtoContract");
        //    //INamedTypeSymbol dataModelContractSymbol = compilation.GetTypeByMetadataName("DataModel");

        //    // using the context, get a list of syntax trees in the users compilation
        //    IEnumerable<SyntaxTree> syntaxTrees = context.Compilation.SyntaxTrees;

        //    // add the filepath of each tree to the class we're building
        //    //foreach (SyntaxTree tree in syntaxTrees)
        //    //{
        //    //    mDebuggerLog.DebugAppendLog($"syntaxTrees: {tree.FilePath}");
        //    //    //sourceBuilder.AppendLine($@"Console.WriteLine(@"" - {tree.FilePath}"");");



        //    //    //System.Diagnostics.Debugger.Launch();
        //    //}
        //    sourceBuilder.AppendLine($"RegisterType(typeof({classSyntax.GetFullName()}));");

        //    // finish creating the source to inject
        //    sourceBuilder.Append(@"
        //            Console.WriteLine(""End register type:"");
        //            }
        //        }
        //    }");

        //    context.AddSource("ProtoBufHelper.Generated.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));

        //    mDebuggerLog.DebugSaveFile($"ProtoBufHelper.Generated-{DateTime.Now.Ticks}.cs", sourceBuilder.ToString());
        //}

        /// <summary>
        /// Created on demand before each generation pass
        /// </summary>
        private class SyntaxReceiver
            : ISyntaxReceiver
        {
            private readonly DebuggerLog mDebuggerLog;

            //public List<InterfaceDeclarationSyntax> Interfaces { get; } = new List<InterfaceDeclarationSyntax>();

            public List<ClassDeclarationSyntax> Classes { get; } = new List<ClassDeclarationSyntax>();

            public SyntaxReceiver(DebuggerLog debuggerLog)
            {
                mDebuggerLog = debuggerLog;
            }

            private readonly object lockObject = new object();
            /// <summary>
            /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
            /// </summary>
            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                lock (lockObject)
                {
//                    if (syntaxNode is CompilationUnitSyntax compilationUnitSyntax)
//                    //&& compilationUnitSyntax.AttributeLists.Any())
//                    {
//                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.CompilationUnitSyntax:");
//                    }
//                    else if (syntaxNode is NamespaceDeclarationSyntax namespaceDeclarationSyntax)
//                    //&& compilationUnitSyntax.AttributeLists.Any())
//                    {
//                        var usings = namespaceDeclarationSyntax.Usings.ToArray();
//                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.NamespaceDeclarationSyntax:");
//                    }
//                    else if (syntaxNode is InterfaceDeclarationSyntax interfaceDeclarationSyntax)
//                    //&& interfaceDeclarationSyntax.AttributeLists.Any())
//                    {
//                        //Interfaces.Add(interfaceDeclarationSyntax);
//                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.InterfaceDeclarationSyntax:");
//                    }
//                    else if (syntaxNode is FieldDeclarationSyntax fieldDeclarationSyntax)
//                    //&& fieldDeclarationSyntax.AttributeLists.Any())
//                    {
//                        //CandidateFields.Add(fieldDeclarationSyntax);
//                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.FieldDeclarationSyntax:");
//                    }
                    //else
                    if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
                    //&& fieldDeclarationSyntax.AttributeLists.Any())
                    {
                        if (classDeclarationSyntax.GetFullName().Contains("RootViewModel"))
                        {
#if DEBUG
                            //System.Diagnostics.Debugger.Launch();
#endif

                            var members = classDeclarationSyntax.Members.ToArray();
                            var constraintClauses = classDeclarationSyntax.ConstraintClauses.ToArray();
                            var types = classDeclarationSyntax.BaseList?.Types.ToArray();
                            var modifiers = classDeclarationSyntax.Modifiers.ToArray();
                            var parent = classDeclarationSyntax.Parent;
                            var parentNamespace = classDeclarationSyntax.Parent as NamespaceDeclarationSyntax;
                            var identifierValue = classDeclarationSyntax.Identifier.Value;
                            var identifierValueText = classDeclarationSyntax.Identifier.ValueText;

                            if (classDeclarationSyntax.AttributeLists.Any(x => x.Attributes.Any(a => a.Name.ToString().Equals("ProtoContract"))))
                            {
                                Classes.Add(classDeclarationSyntax);
                            }

                            var constructors = classDeclarationSyntax.Members.Where(c => c is ConstructorDeclarationSyntax).Cast<ConstructorDeclarationSyntax>();

                            foreach(var constructor in constructors)
                            {
                                var attributes = constructor.AttributeLists.ToArray();
                                var ParameterList = constructor.ParameterList.Parameters;
                            }
                            
                            var classFullName = classDeclarationSyntax.GetFullName();

                            mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.ClassDeclarationSyntax:");
                        }
                    }
                    //                    else if (syntaxNode is QualifiedNameSyntax qualifiedNameSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.QualifiedNameSyntax:");
                    //                    }
                    //                    else if (syntaxNode is IdentifierNameSyntax identifierNameSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.IdentifierNameSyntax:");
                    //                    }
                    //                    else if (syntaxNode is UsingDirectiveSyntax usingDirectiveSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.UsingDirectiveSyntax:");
                    //                    }
                    //                    else if (syntaxNode is AttributeListSyntax attributeListSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.AttributeListSyntax:");
                    //                    }
                    //                    else if (syntaxNode is AttributeArgumentListSyntax attributeArgumentListSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.AttributeArgumentListSyntax:");
                    //                    }
                    //                    else if (syntaxNode is AttributeArgumentSyntax attributeArgumentSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.AttributeArgumentSyntax:");
                    //                    }
                    //                    else if (syntaxNode is NameEqualsSyntax nameEqualsSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.NameEqualsSyntax:");
                    //                    }
                    //                    else if (syntaxNode is SimpleBaseTypeSyntax simpleBaseTypeSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.SimpleBaseTypeSyntax:");
                    //                    }
                    //                    else if (syntaxNode is AttributeSyntax attributeSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.AttributeSyntax:");
                    //                    }
                    //                    else if (syntaxNode is LiteralExpressionSyntax literalExpressionSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.LiteralExpressionSyntax:");
                    //                    }
                    //                    else if (syntaxNode is MemberAccessExpressionSyntax memberAccessExpressionSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.MemberAccessExpressionSyntax:");
                    //                    }
                    //                    else if (syntaxNode is BaseListSyntax baseListSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.BaseListSyntax:");
                    //                    }
                    //                    else if (syntaxNode is PropertyDeclarationSyntax propertyDeclarationSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.PropertyDeclarationSyntax:");
                    //                    }
                    //                    else if (syntaxNode is AccessorListSyntax accessorListSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.AccessorListSyntax:");
                    //                    }
                    //                    else if (syntaxNode is AccessorDeclarationSyntax accessorDeclarationSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.AccessorDeclarationSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is BlockSyntax blockSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.BlockSyntax:");
                    //                    }
                    //                    else if (syntaxNode is LocalDeclarationStatementSyntax localDeclarationStatementSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.LocalDeclarationStatementSyntax:");
                    //                    }
                    //                    else if (syntaxNode is VariableDeclarationSyntax variableDeclarationSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.VariableDeclarationSyntax:");
                    //                    }
                    //                    else if (syntaxNode is IdentifierNameSyntax IdentifierNameSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.IdentifierNameSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is InterpolationSyntax interpolationSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.InterpolationSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is InterpolatedStringTextSyntax interpolatedStringTextSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.InterpolatedStringTextSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is ArgumentSyntax argumentSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.ArgumentSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is CatchClauseSyntax catchClauseSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.CatchClauseSyntax:");
                    //                    }
                    //                    else if (syntaxNode is NullableTypeSyntax nullableTypeSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.NullableTypeSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is PredefinedTypeSyntax predefinedTypeSyntax)
                    //                    {
                    //                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.PredefinedTypeSyntax:");
                    //                    }
                    //                    else if (syntaxNode is EqualsValueClauseSyntax equalsValueClauseSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.EqualsValueClauseSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is InvocationExpressionSyntax invocationExpressionSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.InvocationExpressionSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is ArrowExpressionClauseSyntax arrowExpressionClauseSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.ArrowExpressionClauseSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is BaseExpressionSyntax baseExpressionSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.BaseExpressionSyntax:");
                    //                        return;
                    //                    }
                    else if (syntaxNode is ConstructorDeclarationSyntax constructorDeclarationSyntax)
                    {
                        mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.ConstructorDeclarationSyntax:");
                        return;
                    }
                    //                    else if (syntaxNode is ParameterListSyntax parameterListSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.ParameterListSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is VariableDeclaratorSyntax variableDeclaratorSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.VariableDeclaratorSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is ArgumentListSyntax argumentListSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.ArgumentListSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is TypeOfExpressionSyntax typeOfExpressionSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.TypeOfExpressionSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is CatchDeclarationSyntax catchDeclarationSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.CatchDeclarationSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is ReturnStatementSyntax returnStatementSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.ReturnStatementSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is CastExpressionSyntax castExpressionSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.CastExpressionSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is ObjectCreationExpressionSyntax objectCreationExpressionSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.ObjectCreationExpressionSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is ExpressionStatementSyntax expressionStatementSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.ExpressionStatementSyntax:");
                    //                        return;
                    //                    }
                    //                    else if (syntaxNode is AssignmentExpressionSyntax assignmentExpressionSyntax)
                    //                    {
                    //                        //mDebuggerLog.DebugAppendLog($"OnVisitSyntaxNode.AssignmentExpressionSyntax:");
                    //                        return;
                    //                    }
                    //                    else
                    //                    {
                    //                        //System.Diagnostics.Debugger.Launch();

                    //                        mDebuggerLog.DebugAppendLog($"aaa-OnVisitSyntaxNode: {syntaxNode.GetType().Name}");
                    //                    }

                    //                    mDebuggerLog.DebugAppendLog(syntaxNode.SyntaxTree.FilePath);
                    //                    mDebuggerLog.DebugAppendLog(syntaxNode.GetText().ToString());
                    //                    mDebuggerLog.DebugAppendLog("---------------------------------------------------------------");
                }
            }
        }

        private class DebuggerLog
        {
            private string trazeFileName = $@"C:\Traze\Nivaes.App.Cross-{DateTime.Now.Ticks}";
            private object lookObject = new object();

            [Conditional("DEBUG")]
            public void DebugSaveFile(string fileName, string contentFile)
            {
                try
                {
                    File.AppendAllText(Path.Combine("C:\\Traze", fileName), contentFile);
                }
                catch
                {
                    System.Diagnostics.Debugger.Launch();
                }
            }

            [Conditional("DEBUG")]
            public void DebugAppendLog(string message)
            {
                lock (lookObject)
                {
                    try
                    {
                        File.AppendAllText($"{trazeFileName}.log", $"{DateTime.Now:T} {message}\n");
                    }
                    catch
                    {
                        DebugAppendLog(1, message);
                    }
                }
            }

            private void DebugAppendLog(int n, string message)
            {
                try
                {
                    File.AppendAllText($"{trazeFileName}-{n}.log", $"{DateTime.Now:T} {message}\n");
                }
                catch
                {
                    DebugAppendLog(n + 1, message);
                }
            }
        }

    }
}
