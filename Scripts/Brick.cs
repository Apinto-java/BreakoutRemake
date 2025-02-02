using Godot;
using System;
using System.Collections.Generic;

namespace Breakout.Scripts.Objects
{
	public partial class Brick : StaticBody2D
	{

		[Export] public BrickColor BrickColor;
		[Export] public BrickMap TextureMap;

		private Sprite2D _sprite;
		public int Score { get; private set; }
		public float SpeedMultiplier { get; private set; }

		public override void _Ready()
		{
			_sprite = GetNode<Sprite2D>("Sprite");
			if (TextureMap != null)
			{
				var entry = TextureMap.GetEntry(BrickColor);
				_sprite.Texture = entry.Texture;
				Score = entry.Score;
				SpeedMultiplier = entry.SpeedMultiplier;
			}
		}

		public void CollidedWithBall()
		{
			QueueFree();
		}
	}

	public enum BrickColor
	{
		BROWN,
		RED,
		PINK,
		GREEN,
		YELLOW
	}
}
