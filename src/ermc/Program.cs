using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CLAP;
using Shields.Erm;
using Shields.GraphViz.Services;
using Shields.GraphViz.Components;
using Shields.GraphViz.Models;

namespace ermc
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser<App>();
            parser.Register.EmptyHelpHandler(Console.WriteLine);
            parser.Register.ErrorHandler(exceptionContext =>
            {
                Console.WriteLine(exceptionContext.Exception.Message);
                exceptionContext.ReThrow = false;
            });
            parser.RunStatic(args);
        }
    }

    class App
    {
        [Verb(Description = "Compile a .erm file to a .dot file.")]
        public static void Compile(
            [Required] string inputErmPath)
        {
            var dotPath = Path.ChangeExtension(inputErmPath, "dot");

            var ermParser = new ErmParser();
            var model = ermParser.ParseFile(inputErmPath);

            var graphName = Path.GetFileNameWithoutExtension(inputErmPath);
            var graph = CreateErmGraph(graphName, model);

            using (var outputStream = File.Create(dotPath))
            using (var writer = new StreamWriter(outputStream))
            {
                graph.WriteTo(writer);
            }
        }

        [Verb(Description = "Render an .erm file to a file of the specified format using Graphviz.")]
        public static void Render(
            [Required] string inputErmPath,
            [DefaultValue(RendererFormats.Svg)] RendererFormats format)
        {
            var outputPath = Path.ChangeExtension(inputErmPath, format.ToString().ToLower());

            var ermParser = new ErmParser();
            var model = ermParser.ParseFile(inputErmPath);

            var graphName = Path.GetFileNameWithoutExtension(inputErmPath);
            var graph = CreateErmGraph(graphName, model);

            var graphvizBin = ConfigurationManager.AppSettings["graphvizBin"];
            IRenderer service = new Renderer(graphvizBin);
            using (var fileStream = File.Create(outputPath))
            {
                service.RunAsync(graph, fileStream, RendererLayouts.Dot, format, CancellationToken.None).Wait();
            }
        }

        private static Graph CreateErmGraph(string name, EntityRelationshipModel model)
        {
            return Graph.Undirected.Named(name)

                .Add(AttributeStatement.Graph
                    .Set("pad", "0.5"))

                .Add(AttributeStatement.Node
                    .Set("shape", "box")
                    .Set("fontname", "Consolas")
                    .Set("fontsize", "12")
                    .Set("margin", "0.04,0.02")
                    .Set("width", "0")
                    .Set("height", "0"))

                .Add(AttributeStatement.Edge
                    .Set("fontname", "Consolas")
                    .Set("fontsize", "12"))

                .AddRange(model.Entities
                    .Select(entity => NodeStatement.For(entity.Identifier)
                        .Set("style", entity.IsUnion ? "dashed" : "filled")))

                .Add(AttributeStatement.Node
                    .Set("shape", "diamond")
                    .Set("style", "filled")
                    .Set("margin", "0,0"))

                .AddRange(model.Relationships
                    .Select(relationship => NodeStatement.For(relationship.FullIdentifier)
                        .Set("label", relationship.Identifier)))

                .AddRange(model.Relationships
                    .SelectMany(relationship => Enumerable.Concat(
                        relationship.BeforeEntities
                            .Select(entity => EdgeStatement.For(entity.Identifier, relationship.FullIdentifier)
                                .Set("label", " " + entity.Cardinality + " ")),
                        relationship.AfterEntities
                            .Select(entity => EdgeStatement.For(relationship.FullIdentifier, entity.Identifier)
                                .Set("label", " " + entity.Cardinality + " ")))))

                .Add(AttributeStatement.Node
                    .Set("label", "")
                    .Set("shape", "triangle")
                    .Set("style", "dashed")
                    .Set("fixedsize", "true")
                    .Set("width", "0.3")
                    .Set("height", "0.35"))

                .AddRange(model.Unions
                    .Select(union => NodeStatement.For(union.FullIdentifier)))

                .Add(AttributeStatement.Edge.Set("style", "dashed"))

                .AddRange(model.Unions
                    .Select(union => EdgeStatement.For(union.BaseEntity.Identifier, new NodeId(union.FullIdentifier, Port.North))))

                .AddRange(model.Unions
                    .SelectMany(union => union.DerivedEntities
                        .Select(entity => EdgeStatement.For(union.FullIdentifier, entity.Identifier))));
        }
    }
}
