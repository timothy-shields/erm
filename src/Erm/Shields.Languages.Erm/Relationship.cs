using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shields.Languages.Erm
{
    public class Relationship
    {
        public string Identifier { get; private set; }
        public IReadOnlyCollection<RelatedEntity> BeforeEntities { get; private set; }
        public IReadOnlyCollection<RelatedEntity> AfterEntities { get; private set; }
        public Relationship(string identifier, IReadOnlyCollection<RelatedEntity> beforeEntities, IReadOnlyCollection<RelatedEntity> afterEntities)
        {
            this.Identifier = identifier;
            this.BeforeEntities = beforeEntities;
            this.AfterEntities = afterEntities;
        }

        public string FullIdentifier
        {
            get
            {
                return string.Join("__",
                    string.Join("_", BeforeEntities.Select(e => e.Identifier)),
                    Identifier,
                    string.Join("_", AfterEntities.Select(e => e.Identifier)));
            }
        }
    }
}
