using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPEG;

namespace Shields.Languages.Erm
{
    public class UnionDeclaration : Declaration
    {
        public EntityExpression BaseEntityExpression { get; private set; }
        public IReadOnlyCollection<EntityExpression> DerivedEntityExpressions { get; private set; }
        public UnionDeclaration(EntityExpression baseEntityExpression, IReadOnlyCollection<EntityExpression> derivedEntityExpressions)
        {
            this.BaseEntityExpression = baseEntityExpression;
            this.DerivedEntityExpressions = derivedEntityExpressions;
        }

        public override string Type
        {
            get { return "Union"; }
        }

        public override IEnumerable<Entity> Entities
        {
            get
            {
                return Enumerable.Concat(
                    new [] { new Entity(BaseEntityExpression.Identifier, true) },
                    DerivedEntityExpressions.Select(e => new Entity(e.Identifier, false)));
            }
        }

        public override IEnumerable<Union> Unions
        {
            get
            {
                yield return new Union(
                    new Entity(BaseEntityExpression.Identifier, true),
                    DerivedEntityExpressions.Select(e => new Entity(e.Identifier, false)).ToList());
            }
        }

        internal static new UnionDeclaration From(AstNode node, Func<AstNode, string> reader)
        {
            var debug = reader(node);
            return new UnionDeclaration(
                EntityExpression.From(node.Children["Base"].Children.Single(), reader),
                node.Children["Derived"].Children.Select(childNode => EntityExpression.From(childNode, reader)).ToList());
        }
    }
}
