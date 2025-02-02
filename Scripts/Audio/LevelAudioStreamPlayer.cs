using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Breakout.Scripts.Audio
{
	public partial class LevelAudioStreamPlayer : AudioStreamPlayer
	{
		[Export] private AudioStream _startGameAudioStream;
		[Export] private AudioStream _impactPaddleStream;
		[Export] private AudioStream _wallImpactStream;
		[Export] private AudioStream _brickImpactStream;
		[Export] private AudioStream _loseAreaStream;
		[Export] private AudioStream _gameOverStream;
		[Export] private AudioStream _gameWonAudioStream;

		private Queue<AudioStream> _audioQueue = new();

		public void OnBallHitPaddle()
		{
			PlayStream(_impactPaddleStream);
		}

		public void OnBallHitBrick()
		{
			PlayStream(_brickImpactStream);
		}

		public void OnBallHitWall()
		{
			PlayStream(_wallImpactStream);
		}

		public void OnBallHitLoseArea()
		{
			PlayStream(_loseAreaStream);
		}

		public void OnStartGame()
		{
			PlayStream(_startGameAudioStream);
		}

		public void OnGameWon()
		{
			PlayStream(_gameWonAudioStream);
		}

		public void OnGameOver()
		{
			PlayStream(_gameOverStream);
		}

		private void PlayStream(AudioStream stream)
		{
			Stream = stream;
			Play();
		}

		public override void _Process(double delta)
		{
			// if (Playing || _audioQueue.Count == 0)
			// 	return;

			// Stream = _audioQueue.Dequeue();
			// Play();
		}
	}
}
