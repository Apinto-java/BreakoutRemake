using Godot;
using System;

namespace Breakout.Scripts.Objects
{
	public partial class Paddle : CharacterBody2D
	{
		[Export] public float Speed = 1000f; // Velocidad del paddle
		[Export] public float LimitLeft = 32f; // Límite izquierdo del área de juego
		[Export] public float LimitRight = 648f; // Límite derecho del área de juego
		[Export] public float SmoothFactor = 8f;
		private bool _enabled = true;


		[Signal]
		public delegate void MovedEventHandler(Vector2 newPosition);

		public Marker2D BallMarker { get; private set; }

		public override void _Ready()
		{
			BallMarker = GetNode<Marker2D>("BallMarker");
		}

		public override void _Process(double delta)
		{
			if (!_enabled)
				return;

			// Obtener la posición X del mouse en coordenadas globales
			float targetX = GetGlobalMousePosition().X;

			// Limitar el movimiento dentro del área de juego
			targetX = Mathf.Clamp(targetX, LimitLeft, LimitRight);

			float newX = Mathf.Lerp(GlobalPosition.X, targetX, (float)delta * SmoothFactor);

			// Determinar la velocidad en función de la dirección
			Velocity = new Vector2((newX - GlobalPosition.X) * Speed * (float)delta, 0);

			// Mover el paddle
			MoveAndSlide();
			EmitSignal(SignalName.Moved, BallMarker.GlobalPosition);
		}

		public void OnBallHitLoseArea()
		{
			Disable();
		}

		public void OnGameWon()
		{
			Disable();
		}

		public void OnGameRestarted()
		{
			_enabled = true;
		}

		private void Disable()
		{
			_enabled = false;
		}
	}
}
