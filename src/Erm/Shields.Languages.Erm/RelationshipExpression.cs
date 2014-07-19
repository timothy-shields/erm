using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPEG;

namespace Shields.Languages.Erm
{
    public class RelationshipExpression
    {
        public string Identifier { get; private set; }
        public RelationshipExpression(string identifier)
        {
            this.Identifier = identifier;
        }

        internal static RelationshipExpression From(AstNode node, Func<AstNode, string> reader)
        {
            return new RelationshipExpression(
                reader(node.Children["Identifier"]));
        }
    }
}
