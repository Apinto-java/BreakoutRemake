using Breakout.Scripts.Global;
using Godot;
using System;

namespace Breakout.Scripts.UserInterface
{
	public partial class MainMenu : Control
	{
		private Button _newGameButton;
		private Button _selectLevelButton;
		private Button _quitButton;

		[Export] private PackedScene _newGameScene;
		[Export] private PackedScene _selectLevelScene;

		public override void _Ready()
		{
			_newGameButton = GetNode<Button>("Panel/MarginContainer/CenterContainer/VBoxContainer/NewGameButton");
			_selectLevelButton = GetNode<Button>("Panel/MarginContainer/CenterContainer/VBoxContainer/SelectLevelButton");
			_quitButton = GetNode<Button>("Panel/MarginContainer/CenterContainer/VBoxContainer/QuitButton");

			_newGameButton.Pressed += OnNewGameButtonPressed;
			_selectLevelButton.Pressed += OnSelectLevelButtonPressed;
			_quitButton.Pressed += OnQuitButtonPressed;
		}

		private void OnNewGameButtonPressed()
		{
			var levelManager = GetNode<LevelManager>(GlobalUtils.LevelManagerPath);
			levelManager.NewGame();
		}

		private void OnSelectLevelButtonPressed()
		{
			var levelManager = GetNode<LevelManager>(GlobalUtils.LevelManagerPath);
			levelManager.GoToSelectLevel();
		}

		private void OnQuitButtonPressed()
		{
			GetTree().Quit();
		}
	}
}