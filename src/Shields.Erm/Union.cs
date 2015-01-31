using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shields.Erm
{
    public class Union
    {
        public Entity BaseEntity { get; private set; }
        public IReadOnlyCollection<Entity> DerivedEntities { get; private set; }
        public Union(Entity baseEntity, IReadOnlyCollection<Entity> derivedEntities)
        {
            this.BaseEntity = baseEntity;
            this.DerivedEntities = derivedEntities;
        }

        public string FullIdentifier
        {
            get
            {
                return string.Join("__",
                    BaseEntity.Identifier,
                    "union",
                    string.Join("_", DerivedEntities.Select(e => e.Identifier)));
            }
        }
    }
}
