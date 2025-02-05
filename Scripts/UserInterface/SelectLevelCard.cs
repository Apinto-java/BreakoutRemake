using Godot;
using System;

namespace Breakout.Scripts.UserInterface
{
	public partial class SelectLevelCard : Panel
	{
		private string _levelName;
		private Texture2D _levelTexture;

		public string LevelName
		{
			get => _levelName;
			set
			{
				if (_levelName != value)
				{
					_levelName = value;
					OnLevelNameChanged(value);
				}
			}
		}

		public Texture2D LevelTexture
		{
			get => _levelTexture;
			set
			{
				if (_levelTexture != value)
				{
					_levelTexture = value;
					OnLevelTextureChanged(value);
				}
			}
		}

		public string ScenePath { get; set; }

		[Export] private Label _levelNameLabel;
		[Export] private TextureRect _levelTexture2D;
		[Export] private Button _selectButton { get; set; }

		[Signal]
		public delegate void SelectedEventHandler(string scenePath);

		public override void _Ready()
		{
			_selectButton.Pressed += OnSelectedPressed;
		}

		private void OnSelectedPressed()
		{
			EmitSignal(SignalName.Selected, ScenePath);
		}

		private void OnLevelNameChanged(string newLevelName)
		{
			_levelNameLabel.Text = newLevelName;
		}

		private void OnLevelTextureChanged(Texture2D newLevelTexture)
		{
			_levelTexture2D.Texture = newLevelTexture;
		}
	}
}
