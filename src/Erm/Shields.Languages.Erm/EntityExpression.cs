using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPEG;
using NPEG.Extensions;

namespace Shields.Languages.Erm
{
    public class EntityExpression
    {
        public string Identifier { get; private set; }
        public EntityExpression(string identifier)
        {
            this.Identifier = identifier;
        }

        internal static EntityExpression From(AstNode node, Func<AstNode, string> reader)
        {
            return new EntityExpression(
                reader(node.Children["Identifier"]));
        }
    }
}
