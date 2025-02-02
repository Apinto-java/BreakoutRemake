using Godot;
using System;

namespace Breakout.Scripts.Objects
{
	public partial class PaddleSprite : Sprite2D
	{
		[Export] private Texture2D _normalTexture;
		[Export] private Texture2D _textureWhenHit;
		[Export] private float _hitDuration;
		private Timer _timer;

		public void Hit()
		{
			Texture = _textureWhenHit;

			_timer = new Timer()
			{
				WaitTime = _hitDuration,
			};
			_timer.Timeout += OnHitTimerTimeout;
			AddChild(_timer);
			_timer.Start();
		}

		private void OnHitTimerTimeout()
		{
			_timer.Stop();
			Texture = _normalTexture;

			_timer.Timeout -= OnHitTimerTimeout;
			_timer.QueueFree();
			_timer = null;
		}
	}
}
