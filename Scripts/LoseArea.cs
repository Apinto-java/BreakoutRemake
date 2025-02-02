using Godot;
using System;

namespace Breakout.Scripts.Objects
{
	public partial class LoseArea : Area2D
	{
		[Signal]
		public delegate void GameOverEventHandler();

		public void OnBallEntered(Node2D node)
		{
			EmitSignal(SignalName.GameOver);
		}
	}
}

