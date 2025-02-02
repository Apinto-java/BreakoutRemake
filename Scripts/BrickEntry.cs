using Breakout.Scripts.Objects;
using Godot;
using System;


[GlobalClass]
[Tool]
public partial class BrickEntry : Resource
{
    [Export] public BrickColor BrickColor;
    [Export] public Texture2D Texture;
    [Export] public int Score;
    [Export] public float SpeedMultiplier;
}
