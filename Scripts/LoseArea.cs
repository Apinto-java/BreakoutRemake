using Godot;
using System;

namespace Breakout.Scripts.Objects
{
	public partial class LoseArea : Area2D
	{
		[Signal]
		public delegate void GameOverEventHandler();

		public override void _Ready()
		{
		}

		public override void _Process(double delta)
		{
		}

		public void OnBallEntered(Node2D node)
		{
			EmitSignal(SignalName.GameOver);
		}
	}
}

