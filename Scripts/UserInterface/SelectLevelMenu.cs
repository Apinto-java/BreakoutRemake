using Breakout.Scripts.Global;
using Breakout.Scripts.Resources;
using Godot;
using System;

namespace Breakout.Scripts.UserInterface
{
	public partial class SelectLevelMenu : CanvasLayer
	{
		[Export] private LevelMap _levelMap;
		[Export] private GridContainer _cardsContainer;
		[Export] private PackedScene _selectLevelCard;
		[Export] private Button _backButton;

		public override void _Ready()
		{
			foreach (var level in _levelMap.Levels)
			{
				var card = _selectLevelCard.Instantiate() as SelectLevelCard;
				card.LevelName = level.Name;
				card.LevelTexture = level.Image;
				card.ScenePath = level.Scene.ResourcePath;
				card.SizeFlagsHorizontal = Control.SizeFlags.Expand;

				card.Selected += OnSelectedScene;

				_cardsContainer.AddChild(card);
			}
			_cardsContainer.SizeFlagsHorizontal = Control.SizeFlags.Expand;

			_backButton.Pressed += OnBackButtonPressed;
		}

		private void OnBackButtonPressed()
		{
			var levelManager = GetNode<LevelManager>(GlobalUtils.LevelManagerPath);
			levelManager.GoToMainMenu();
		}

		private void OnSelectedScene(string scenePath)
		{
			var levelManager = GetNode<LevelManager>(GlobalUtils.LevelManagerPath);
			levelManager.GoToSelectedLevel(scenePath);
		}

	}
}
