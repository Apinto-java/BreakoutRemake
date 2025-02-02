using Breakout.Scripts.Audio;
using Breakout.Scripts.UserInterface;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Breakout.Scripts.Objects
{
	public partial class Main : Node2D
	{
		private Ball _ball;
		private Paddle _paddle;
		private LoseArea _loseArea;
		private UI _ui;
		private Marker2D _playerSpawn;
		private LevelAudioStreamPlayer _levelAudioStreamPlayer;
		private Timer _loseAreaTimer;

		private List<Brick> _bricks = new();
		private bool _gameStarted = false;
		private bool _gamePaused = false;
		private int _score = 0;
		[Export] private int _lives = 4;
		[Export] private float _timeAfterTouchGround = 2f;
		[Export] private float _timeForGameOver = 1f;
		[Export] private float _timeForGameWon = 1f;
		private Timer _gameOverTimer;
		private Timer _gameWonTimer;

		[Signal]
		public delegate void ScoreChangedEventHandler(int newScore);
		[Signal]
		public delegate void LivesChangedEventHandler(int newLives);
		[Signal]
		public delegate void StartGameEventHandler();
		[Signal]
		public delegate void GameRestartedEventHandler();
		[Signal]
		public delegate void BallHitGroundEventHandler();
		[Signal]
		public delegate void GameWonEventHandler();
		[Signal]
		public delegate void GameOverEventHandler();

		public override void _Ready()
		{
			_ball = GetNode<Ball>("Ball");
			_paddle = GetNode<Paddle>("Paddle");
			_loseArea = GetNode<LoseArea>("LoseArea");
			_ui = GetNode<UI>("UI");
			_playerSpawn = GetNode<Marker2D>("PlayerSpawnPosition");
			_levelAudioStreamPlayer = GetNode<LevelAudioStreamPlayer>("LevelAudioStreamPlayer");
			InitializeBricks();
			InitializeLoseAreaTimer();
			InitializeGameWonTimer();
			InitializeGameOverTimer();

			// Game States events
			StartGame += _ball.OnGameStarted;
			StartGame += _levelAudioStreamPlayer.OnStartGame;
			GameRestarted += _paddle.OnGameRestarted;
			GameOver += _levelAudioStreamPlayer.OnGameOver;
			GameWon += _paddle.OnGameWon;
			GameWon += _ball.OnGameWon;
			GameWon += _levelAudioStreamPlayer.OnGameWon;
			// Object events
			_paddle.Moved += _ball.OnPaddleMoved;
			_ball.HitBrick += OnBallHitBrick;
			BallHitGround += _ball.OnHitGround;
			BallHitGround += _paddle.OnBallHitLoseArea;
			_loseArea.BodyEntered += _loseArea.OnBallEntered;
			_loseArea.GameOver += OnBallHitLoseArea;
			_ball.HitPaddle += _levelAudioStreamPlayer.OnBallHitPaddle;
			_ball.HitWall += _levelAudioStreamPlayer.OnBallHitWall;
			// UI Events
			ScoreChanged += _ui.OnScoreChanged;
			LivesChanged += _ui.OnLivesChanged;
			GameOver += _ui.OnGameOver;
			GameWon += _ui.OnGameWon;
			SetInitialLives();
		}

		/// <summary>
		/// Gets all the nodes that are in the "bricks" group and adds them to the <c>bricks</c> list
		/// </summary>
		private void InitializeBricks()
		{
			var bricks = GetTree().GetNodesInGroup("bricks")
				.Where(b => b is Brick)
				.Select(b => (Brick)b);

			foreach (var brick in bricks)
			{
				_bricks.Add(brick);
			}
		}

		/// <summary>
		/// Initilizes the <c>loseAreaTimer</c>
		/// </summary>
		private void InitializeLoseAreaTimer()
		{
			_loseAreaTimer = new Timer()
			{
				WaitTime = _timeAfterTouchGround,
				Autostart = false
			};
			_loseAreaTimer.Timeout += OnLoseAreaTimerTimeout;
			AddChild(_loseAreaTimer);
		}

		/// <summary>
		/// Initializes the <c>gameWonTimer</c>.
		/// </summary>
		private void InitializeGameWonTimer()
		{
			_gameWonTimer = new Timer()
			{
				WaitTime = _timeForGameWon,
				Autostart = false
			};
			_gameWonTimer.Timeout += OnGameWonTimerTimeout;
			AddChild(_gameWonTimer);
		}

		/// <summary>
		/// Stops the <c>gameWonTimer</c> and emits the <c>GameWon</c> Signal.
		/// </summary>
		private void OnGameWonTimerTimeout()
		{
			_gameWonTimer.Stop();
			_gameWonTimer.Timeout -= OnGameWonTimerTimeout;
			_gameWonTimer.QueueFree();
			_gameWonTimer = null;
			InitializeGameWonTimer();

			GD.Print("You won!");
			EmitSignal(SignalName.GameWon);
		}

		/// <summary>
		/// Emits the <c>LivesChanged</c> signal at the start of the level, updating the UI
		/// </summary>
		private void SetInitialLives()
		{
			EmitSignal(SignalName.LivesChanged, _lives);
		}

		/// <summary>
		/// Executed each frame. Checks if the start button has been pressed, if it has, and the game has not started
		/// it set the "started" variable as true and Emits the "StartGame" Signal
		/// </summary>
		/// <param name="delta">Time elapsed since last frame</param>
		public override void _Process(double delta)
		{
			if (!Input.IsActionJustPressed("start") || _gameStarted || _gamePaused || _lives <= 0)
				return;

			_gameStarted = true;
			EmitSignal(SignalName.StartGame);
		}

		/// <summary>
		/// Method intended to be called every time the Ball enteres the Lose Area.
		/// </summary>
		private void OnBallHitLoseArea()
		{
			_lives = Mathf.Max(_lives - 1, 0);
			EmitSignal(SignalName.LivesChanged, _lives);
			_levelAudioStreamPlayer.OnBallHitLoseArea();

			_gameStarted = false;
			_gamePaused = true;
			EmitSignal(SignalName.BallHitGround);
			_loseAreaTimer.Start();

			if (_lives == 0)
			{
				OnGameOver();
			}
		}

		/// <summary>
		/// Method intended to be called when the <c>loseAreaTimer</c> reaches 0.
		/// </summary>
		private void OnLoseAreaTimerTimeout()
		{
			_loseAreaTimer.Timeout -= OnLoseAreaTimerTimeout;
			_loseAreaTimer.QueueFree();
			_loseAreaTimer = null;
			InitializeLoseAreaTimer();

			if (_lives > 0)
			{
				EmitSignal(SignalName.GameRestarted);
				_gamePaused = false;
				_paddle.Position = _playerSpawn.Position;
				_ball.Position = _paddle.BallMarker.Position;
			}
		}

		/// <summary>
		/// Method intended to be called when the Ball hits a Brick
		/// </summary>
		/// <param name="scorePoints">Score points achieved by hitting the brick</param>
		private void OnBallHitBrick(Brick brick)
		{
			_score += brick.Score;
			EmitSignal(SignalName.ScoreChanged, _score);

			_levelAudioStreamPlayer.OnBallHitBrick();

			_bricks.Remove(brick);
			if (_bricks.Count == 0)
				_gameWonTimer.Start();
		}

		/// <summary>
		/// Method intended to be called when the game is over. In other words, when <c>lives</c> reach 0
		/// </summary>
		private void OnGameOver()
		{
			_gameOverTimer?.Start();
		}

		/// <summary>
		/// Initializes the <c>gameOverTimer</c>.
		/// </summary>
		private void InitializeGameOverTimer()
		{
			_gameOverTimer = new Timer()
			{
				Autostart = false,
				WaitTime = _timeForGameOver
			};
			_gameOverTimer.Timeout += OnGameOverTimerTimeout;
			AddChild(_gameOverTimer);
		}

		/// <summary>
		/// Method intended to be called every time <c>gameOverTimer</c> reaches 0
		/// </summary>
		private void OnGameOverTimerTimeout()
		{
			_gameOverTimer.Stop();
			_gameOverTimer.Timeout -= OnGameOverTimerTimeout;
			_gameOverTimer.QueueFree();
			_gameOverTimer = null;
			EmitSignal(SignalName.GameOver);
			InitializeGameOverTimer();
		}
	}
}
