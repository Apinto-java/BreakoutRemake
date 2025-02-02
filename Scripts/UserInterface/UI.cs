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
		private Button _goBackToMenuButton;
		private Button _quitGameButton;

		public override void _Ready()
		{
			_scoreLabel = GetNode<Label>("VBoxContainer/MarginContainer/HBoxContainer/ScoreLabel");
			_livesLabel = GetNode<Label>("VBoxContainer/MarginContainer/HBoxContainer/LivesLabel");
			_gameEndedMenu = GetNode<CenterContainer>("VBoxContainer/GameEndedMenu");
			_tryAgainButton = GetNode<Button>("VBoxContainer/GameEndedMenu/VBoxContainer/TryAgainButton");
			_goBackToMenuButton = GetNode<Button>("VBoxContainer/GameEndedMenu/VBoxContainer/GoBackToMenuButton");
			_quitGameButton = GetNode<Button>("VBoxContainer/GameEndedMenu/VBoxContainer/QuitButton");

			_gameEndedMenu.Visible = false;
			_tryAgainButton.Pressed += OnTryAgainPressed;
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
			_tryAgainButton.Visible = !won;
		}

		private void OnTryAgainPressed()
		{
			GetTree().ReloadCurrentScene();
		}

		private void OnGoBackToMenuPressed()
		{

		}

		private void OnQuitGamePressed()
		{
			GetTree().Quit();
		}
	}
}