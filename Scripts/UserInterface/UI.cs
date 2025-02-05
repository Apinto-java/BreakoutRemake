using Breakout.Scripts.Global;
using Godot;
using System;

namespace Breakout.Scripts.UserInterface
{
	public partial class UI : CanvasLayer
	{
		private Label _scoreLabel;
		private Label _livesLabel;

		private CenterContainer _gameEndedMenu;


		private Button _tryAgainButton;
		private Button _nextLevelButton;
		private Button _goBackToMenuButton;
		private Button _quitGameButton;

		public override void _Ready()
		{
			_scoreLabel = GetNode<Label>("VBoxContainer/MarginContainer/HBoxContainer/ScoreLabel");
			_livesLabel = GetNode<Label>("VBoxContainer/MarginContainer/HBoxContainer/LivesLabel");
			_gameEndedMenu = GetNode<CenterContainer>("VBoxContainer/GameEndedMenu");
			_tryAgainButton = GetNode<Button>("VBoxContainer/GameEndedMenu/VBoxContainer/TryAgainButton");
			_nextLevelButton = GetNode<Button>("VBoxContainer/GameEndedMenu/VBoxContainer/NextLevelButton");
			_goBackToMenuButton = GetNode<Button>("VBoxContainer/GameEndedMenu/VBoxContainer/GoBackToMenuButton");
			_quitGameButton = GetNode<Button>("VBoxContainer/GameEndedMenu/VBoxContainer/QuitButton");

			_gameEndedMenu.Visible = false;
			_tryAgainButton.Pressed += OnTryAgainPressed;
			_nextLevelButton.Pressed += OnNextLevelPressed;
			_goBackToMenuButton.Pressed += OnGoBackToMenuPressed;
			_quitGameButton.Pressed += OnQuitGamePressed;
		}

		public void OnScoreChanged(int newScore)
		{
			if (_scoreLabel == null)
				return;

			_scoreLabel.Text = $"Score: {newScore}";
		}

		public void OnLivesChanged(int newLives)
		{
			if (_livesLabel == null)
				return;

			_livesLabel.Text = $"Lives: {newLives}";
		}

		public void OnGameWon()
		{
			OnGameEnded(true);
		}

		public void OnGameOver()
		{
			OnGameEnded(false);
		}

		private void OnGameEnded(bool won)
		{
			_gameEndedMenu.Visible = true;
			var levelManager = GetNode<LevelManager>(GlobalUtils.LevelManagerPath);
			_tryAgainButton.Visible = !won;
			_nextLevelButton.Visible = won && levelManager.HasNextLevel();
		}

		private void OnTryAgainPressed()
		{
			GetTree().ReloadCurrentScene();
		}

		private void OnNextLevelPressed()
		{
			var levelManager = GetNode<LevelManager>(GlobalUtils.LevelManagerPath);
			levelManager.GoToNextLevel();
		}

		private void OnGoBackToMenuPressed()
		{
			var levelManager = GetNode<LevelManager>(GlobalUtils.LevelManagerPath);
			levelManager.GoToMainMenu();
		}

		private void OnQuitGamePressed()
		{
			GetTree().Quit();
		}
	}
}