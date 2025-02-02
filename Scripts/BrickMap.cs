using Breakout.Scripts.Objects;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


[GlobalClass]
[Tool]
public partial class BrickMap : Resource
{
    [Export] public Godot.Collections.Array<BrickEntry> BrickTextures = new();

    public BrickEntry GetEntry(BrickColor color)
    {
        return BrickTextures.FirstOrDefault(entry => entry.BrickColor == color);
    }
}

