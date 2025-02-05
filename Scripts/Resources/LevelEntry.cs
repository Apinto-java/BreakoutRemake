using Godot;
using System;

namespace Breakout.Scripts.Resources
{
    [GlobalClass]
    [Tool]
    public partial class LevelEntry : Resource
    {
        [Export] public string Name { get; set; }
        [Export] public PackedScene Scene { get; set; }
        [Export] public Texture2D Image { get; set; }
        [Export] public int Order { get; set; } = 0;
    }
}