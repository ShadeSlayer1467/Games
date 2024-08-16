using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicGameInterface
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class GameNameAttribute : Attribute
    {
        public string Name { get; }

        public GameNameAttribute(string name)
        {
            Name = name;
        }
    }

}
