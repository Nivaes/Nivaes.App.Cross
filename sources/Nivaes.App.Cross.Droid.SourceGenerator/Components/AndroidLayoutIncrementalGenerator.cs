//namespace Nivaes.App.Cross
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Text;
//    using System.Xml.Linq;
//    using Microsoft.CodeAnalysis;
//    using Microsoft.CodeAnalysis.Text;
//    using System.Linq;

//    [Generator]
//    internal class AndroidLayoutIncrementalGenerator : IIncrementalGenerator
//    {
//        public void Initialize(IncrementalGeneratorInitializationContext context)
//        {
////#if DEBUG
////            System.Diagnostics.Debugger.Launch();
////#endif

//            var layoutFiles = context.AdditionalTextsProvider
//                .Where(file =>
//                {
////#if DEBUG
////                    System.Diagnostics.Debugger.Launch();
////#endif

//                    var aa = file.Path.Contains("\\Resources\\layout\\") && file.Path.EndsWith(".xml");
//                    return aa;
//                });

//            //var compilationProvider = context.;
//            //compilationProvider.SelectMany(a => a.name)

//            context.RegisterSourceOutput(layoutFiles, GenerateLayoutCode);



//            //var compilationProvider = context.CompilationProvider;
//            ////compilationProvider.Select(
//            ////    var aa = a => a.AssemblyName
//            ////    );

//            //context.RegisterSourceOutput(compilationProvider, (ctx, compilation) =>
//            //{
//            //    //ProcessAllFiles(ctx, compilation);
//            //});
//        }

//        private void ProcessAllFiles(SourceProductionContext context, Compilation compilation)
//        {
//            // Obtener todas las unidades de sintaxis del proyecto
//            var syntaxTrees = compilation.SyntaxTrees;

//            foreach (var syntaxTree in syntaxTrees)
//            {
//                // Leer el archivo fuente
//                var root = syntaxTree.GetRoot();

//                // Nombre del archivo
//                var filePath = syntaxTree.FilePath;

//                // Opcional: Procesar nodos específicos del archivo (clases, métodos, etc.)
//                var classes = root.DescendantNodes()
//                                  //.OfType<ClassDeclarationSyntax>()
//                                  //.Select(cls => cls.Identifier.Text)
//                                  .ToList();

//                // Ejemplo: Emitir un log con las clases encontradas
//                //context.ReportDiagnostic(Diagnostic.Create(
//                //    new DiagnosticDescriptor("PF001", "Clase Detectada", $"Archivo: {filePath}, Clases: {string.Join(", ", classes)}",
//                //    "ProjectFiles", DiagnosticSeverity.Info, true), Location.None));
//            }
//        }

//        private void GenerateLayoutCode(SourceProductionContext context, AdditionalText layoutFile)
//        {
//            // Leer el contenido del archivo XML
//            var fileContent = layoutFile.GetText(context.CancellationToken)?.ToString();
//            if (string.IsNullOrEmpty(fileContent))
//                return;

//            // Analizar el archivo XML
//            var document = XDocument.Parse(fileContent);
//            var rootElement = document.Root;
//            if (rootElement == null) return;

//            // Extraer vistas del layout
//            var views = rootElement.Descendants()
//                .Select(node => new
//                {
//                    ViewName = node.Name.LocalName,
//                    Id = node.Attributes().FirstOrDefault(attr => attr.Name.LocalName == "id")?.Value,
//                    Width = node.Attributes().FirstOrDefault(attr => attr.Name.LocalName == "layout_width")?.Value,
//                    Height = node.Attributes().FirstOrDefault(attr => attr.Name.LocalName == "layout_height")?.Value
//                })
//                .Where(view => view.Id != null)
//                .ToList();

//            if (!views.Any())
//                return;

//            // Generar el código C#
//            var layoutName = System.IO.Path.GetFileNameWithoutExtension(layoutFile.Path);
//            var builder = new StringBuilder();
//            builder.AppendLine("using System;");
//            builder.AppendLine("namespace Generated.Layouts");
//            builder.AppendLine("{");
//            builder.AppendLine($"    public class {layoutName}");
//            builder.AppendLine("    {");

//            foreach (var view in views)
//            {
//                var id = view.Id.Replace("@+id/", "").Replace("@id/", "");
//                builder.AppendLine($"        public string {id} => \"{id}\"; // View: {view.ViewName}, Width: {view.Width}, Height: {view.Height}");
//            }

//            builder.AppendLine("    }");
//            builder.AppendLine("}");

//            // Emitir el código generado
//            context.AddSource($"{layoutName}.g.cs", SourceText.From(builder.ToString(), Encoding.UTF8));
//        }
//    }
//}
