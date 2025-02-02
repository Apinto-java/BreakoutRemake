using Godot;
using System;

namespace Breakout.Scripts.Objects
{
	public partial class Ball : CharacterBody2D
	{

		[Export] private float _speed = 200;
		[Export] private float _maxSpeed = 400;
		public bool Enabled { get; set; } = false;

		private RandomNumberGenerator rng = new RandomNumberGenerator();

		[Signal]
		public delegate void HitBrickEventHandler(Brick brick);
		[Signal]
		public delegate void HitPaddleEventHandler();
		[Signal]
		public delegate void HitWallEventHandler();

		public override void _Ready()
		{
		}

		public override void _Process(double delta)
		{
			if (!Enabled)
				return;

			var collision = MoveAndCollide(Velocity * (float)delta);

			if (collision == null || collision.GetCollider() == null)
				return;

			HandleCollision(collision);
		}

		private void HandleCollision(KinematicCollision2D collision)
		{
			Velocity = Velocity.Bounce(collision.GetNormal());

			// If the collision was with a Brick
			if (collision.GetCollider() is Brick brick)
			{
				EmitSignal(SignalName.HitBrick, brick);
				var multiplier = brick.SpeedMultiplier;
				brick.CollidedWithBall();
				Velocity = (Velocity * multiplier).Length() > _maxSpeed ? Velocity.Normalized() * _maxSpeed : Velocity * multiplier;
			}

			// If the collision was with a Paddle
			if (collision.GetCollider() is Paddle)
			{
				HandlePaddleHit(collision);
				return;
			}

			if (collision.GetCollider() is Wall)
			{
				EmitSignal(SignalName.HitWall);
			}

			// If the collider was a wall
			if (IsStraightTrajectory())
			{
				float angleVariation = Mathf.DegToRad(rng.RandfRange(-35, 35));
				Velocity = Velocity.Rotated(angleVariation);
			}
		}

		private void HandlePaddleHit(KinematicCollision2D collision)
		{
			var shape = collision.GetColliderShape();

			if (shape is not PaddleCollisionShape paddleCollisionShape)
				return;

			EmitSignal(SignalName.HitPaddle);

			if (shape is LeftCollisionShape)
			{
				Velocity = new Vector2(-Mathf.Abs(Velocity.X), Velocity.Y);
			}

			if (shape is MiddleCollisionShape)
			{
				float speed = Velocity.Length();
				Velocity = Vector2.Up * speed;
			}

			if (shape is RightCollisionShape)
			{
				Velocity = new Vector2(Mathf.Abs(Velocity.X), Velocity.Y);
			}

			paddleCollisionShape.Hit();
		}

		private bool IsStraightTrajectory()
		{
			if (Velocity.Length() == 0)
				return false;

			Vector2[] directions = { Vector2.Right, Vector2.Left, Vector2.Up, Vector2.Down };

			foreach (var dir in directions)
			{
				float angle = Mathf.RadToDeg(Velocity.AngleTo(dir));
				if (Mathf.Abs(angle) <= 5.0f)
					return true;
			}

			return false;
		}

		public void OnPaddleMoved(Vector2 newPosition)
		{
			if (!Enabled)
				GlobalPosition = newPosition;
		}

		public void OnGameStarted()
		{
			Enabled = true;
			Velocity = Vector2.Up * _speed;
		}

		public void OnHitGround()
		{
			Disable();
		}

		public void OnGameWon()
		{
			Disable();
		}

		private void Disable()
		{
			Enabled = false;
			Velocity = Vector2.Zero;
		}
	}
}
