using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NPEG;
using NPEG.Extensions;
using NPEG.GrammarInterpreter;

namespace Shields.Languages.Erm
{
    public class ErmParser
    {
        private static readonly Lazy<AExpression> grammar = new Lazy<AExpression>(LoadGrammar);

        private static AExpression Grammar
        {
            get { return grammar.Value; }
        }

        private static AExpression LoadGrammar()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Shields.Languages.Erm.Grammar.txt";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                string rules = reader.ReadToEnd();
                return PEGrammar.Load(rules);
            }
        }

        public EntityRelationshipModel ParseFile(string ermPath)
        {
            using (var inputStream = File.OpenRead(ermPath))
            {
                var input = File.ReadAllText(ermPath);
                return ParseText(input);
            }
        }

        public EntityRelationshipModel ParseStream(Stream ermStream)
        {
            using (var reader = new StreamReader(ermStream))
            {
                var input = reader.ReadToEnd();
                return ParseText(input);
            }
        }

        public EntityRelationshipModel ParseText(string erm)
        {
            var iterator = new StringInputIterator(erm);
            var visitor = new NpegParserVisitor(iterator);
            Grammar.Accept(visitor);
            if (!visitor.IsMatch
                || visitor.AST.Token.End - visitor.AST.Token.Start + 1 < erm.Length)
            {
                throw new Exception("Failed to parse input file.");
            }
            return EntityRelationshipModel.From(visitor.AST, node => node.Token.ValueAsString(iterator));
        }
    }
}
