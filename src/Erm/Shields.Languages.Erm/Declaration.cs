using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPEG;

namespace Shields.Languages.Erm
{
    public abstract class Declaration
    {
        public abstract string Type { get; }
        public virtual IEnumerable<Entity> Entities
        {
            get
            {
                return Enumerable.Empty<Entity>();
            }
        }
        public virtual IEnumerable<Relationship> Relationships
        {
            get
            {
                return Enumerable.Empty<Relationship>();
            }
        }
        public virtual IEnumerable<Union> Unions
        {
            get
            {
                return Enumerable.Empty<Union>();
            }
        }

        internal static Declaration From(AstNode node, Func<AstNode, string> reader)
        {
            switch (node.Token.Name)
            {
                case "RelationshipDeclaration":
                    return RelationshipDeclaration.From(node, reader);
                case "UnionDeclaration":
                    return UnionDeclaration.From(node, reader);
                default:
                    throw new Exception("Unrecognized token name: " + node.Token.Name);
            }
        }
    }
}
