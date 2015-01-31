using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPEG;

namespace Shields.Erm
{
    public class EntityRelationshipModel
    {
        public IReadOnlyCollection<Declaration> Declarations { get; private set; }
        public EntityRelationshipModel(IReadOnlyCollection<Declaration> declarations)
        {
            this.Declarations = declarations;
        }

        internal static EntityRelationshipModel From(AstNode node, Func<AstNode, string> reader)
        {
            return new EntityRelationshipModel(
                node.Children.Select(childNode => Declaration.From(childNode, reader)).ToList());
        }

        public IEnumerable<Entity> Entities
        {
            get
            {
                return Declarations
                    .SelectMany(x => x.Entities)
                    .GroupBy(e => e.Identifier, (identifier, entities) => new Entity(identifier, entities.Any(e => e.IsUnion)));
            }
        }

        public IEnumerable<Relationship> Relationships
        {
            get
            {
                return Declarations.SelectMany(x => x.Relationships);
            }
        }

        public IEnumerable<Union> Unions
        {
            get
            {
                return Declarations.SelectMany(x => x.Unions);
            }
        }
    }
}
