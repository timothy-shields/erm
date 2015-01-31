using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shields.Erm
{
    public class Entity
    {
        public string Identifier { get; private set; }
        public bool IsUnion { get; private set; }
        public Entity(string identifier, bool isUnion)
        {
            this.Identifier = identifier;
            this.IsUnion = isUnion;
        }
    }
}
