using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shields.Languages.Erm
{
    public class RelatedEntity
    {
        public string Identifier { get; private set; }
        public string Cardinality { get; private set; }
        public RelatedEntity(string identifier, string cardinality)
        {
            this.Identifier = identifier;
            this.Cardinality = cardinality;
        }
    }
}
