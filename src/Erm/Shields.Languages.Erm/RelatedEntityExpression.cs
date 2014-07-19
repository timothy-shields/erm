using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPEG;
using NPEG.Extensions;

namespace Shields.Languages.Erm
{
    public class RelatedEntityExpression
    {
        public string Identifier { get; private set; }
        public string Cardinality { get; private set; }
        public RelatedEntityExpression(string identifier, string cardinality)
        {
            this.Identifier = identifier;
            this.Cardinality = cardinality;
        }

        internal static RelatedEntityExpression From(AstNode node, Func<AstNode, string> reader)
        {
            return new RelatedEntityExpression(
                reader(node.Children["Identifier"]),
                reader(node.Children["Cardinality"]));
        }
    }
}
