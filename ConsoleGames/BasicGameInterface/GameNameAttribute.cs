using System;

namespace BasicGameInterface
{
    // Name Used to display the game in the menu when selecting a game to play
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
