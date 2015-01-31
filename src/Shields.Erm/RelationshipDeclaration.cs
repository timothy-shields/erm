using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPEG;

namespace Shields.Erm
{
    public class RelationshipDeclaration : Declaration
    {
        public RelationshipExpression RelationshipExpression { get; private set; }
        public IReadOnlyCollection<RelatedEntityExpression> BeforeEntityExpressions { get; private set; }
        public IReadOnlyCollection<RelatedEntityExpression> AfterEntityExpressions { get; private set; }
        public RelationshipDeclaration(
            RelationshipExpression relationshipExpression,
            IReadOnlyCollection<RelatedEntityExpression> beforeEntityExpressions,
            IReadOnlyCollection<RelatedEntityExpression> afterEntityExpressions)
        {
            this.RelationshipExpression = relationshipExpression;
            this.BeforeEntityExpressions = beforeEntityExpressions;
            this.AfterEntityExpressions = afterEntityExpressions;
        }

        public override string Type
        {
            get { return "Relationship"; }
        }

        public override IEnumerable<Entity> Entities
        {
            get
            {
                return Enumerable.Concat(BeforeEntityExpressions, AfterEntityExpressions)
                    .Select(e => new Entity(e.Identifier, false))
                    .GroupBy(e => e.Identifier, (identifier, group) => group.First());
            }
        }

        public override IEnumerable<Relationship> Relationships
        {
            get
            {
                yield return new Relationship(
                    RelationshipExpression.Identifier,
                    BeforeEntityExpressions.Select(e => new RelatedEntity(e.Identifier, e.Cardinality)).ToList(),
                    AfterEntityExpressions.Select(e => new RelatedEntity(e.Identifier, e.Cardinality)).ToList());
            }
        }

        internal static new RelationshipDeclaration From(AstNode node, Func<AstNode, string> reader)
        {
            return new RelationshipDeclaration(
                RelationshipExpression.From(node.Children["RelationshipExpression"], reader),
                node.Children["Before"].Children.Select(childNode => RelatedEntityExpression.From(childNode, reader)).ToList(),
                node.Children["After"].Children.Select(childNode => RelatedEntityExpression.From(childNode, reader)).ToList());
        }
    }
}
