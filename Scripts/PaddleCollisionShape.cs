using Godot;
using System;

namespace Breakout.Scripts.Objects
{
    public partial class PaddleCollisionShape : CollisionShape2D
    {
        private PaddleSprite _paddleSprite;

        public override void _Ready()
        {
            _paddleSprite = GetNode<PaddleSprite>("PaddleSprite");
        }

        public void Hit()
        {
            if (_paddleSprite != null)
            {
                _paddleSprite.Hit();
            }
        }
    }
}