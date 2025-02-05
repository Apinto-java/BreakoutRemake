using Godot;
using System;
using System.Linq;

namespace Breakout.Scripts.Resources
{
    [GlobalClass]
    [Tool]
    public partial class LevelMap : Resource
    {
        [Export] public Godot.Collections.Array<LevelEntry> Levels = new();

        public LevelEntry GetEntry()
        {
            return null;
        }
    }
}