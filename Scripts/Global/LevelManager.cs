using Breakout.Scripts.Resources;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Breakout.Scripts.Global
{
    public partial class LevelManager : Node
    {

        private const string LEVEL_MAP_RES_PATH = "res://Assets/LevelMap.tres";
        private LevelMap _levelMap;
        private const string MAIN_MENU_SCENE_PATH = "res://UserInterface/main_menu.tscn";
        private const string SELECT_LEVEL_SCENE = "res://UserInterface/select_level_menu.tscn";
        private Node _currentLevel = null;
        private List<string> _scenes = new();

        public override void _Ready()
        {
            _levelMap = GD.Load<LevelMap>(LEVEL_MAP_RES_PATH);
            LevelEntry[] levels = new LevelEntry[_levelMap.Levels.Count];
            _levelMap.Levels.CopyTo(levels, 0);
            _scenes = levels
                .OrderBy(x => x.Order)
                .Select(x => x.Scene.ResourcePath)
                .ToList();

            var root = GetTree().Root;
            _currentLevel = root.GetChild(root.GetChildCount() - 1);
        }

        public void NewGame()
        {
            if (_scenes.Count == 0)
            {
                GD.PrintErr("No level scenes were loaded");
                return;
            }

            CallDeferred(MethodName.DeferredGoToScene, _scenes[0]);
        }

        public void GoToMainMenu()
        {
            CallDeferred(MethodName.DeferredGoToScene, MAIN_MENU_SCENE_PATH);
        }

        public void GoToSelectLevel()
        {
            CallDeferred(MethodName.DeferredGoToScene, SELECT_LEVEL_SCENE);
        }

        public void GoToNextLevel()
        {
            var currentIdx = _scenes.IndexOf(_currentLevel.SceneFilePath);
            var idx = Mathf.Min(_scenes.Count - 1, currentIdx + 1);
            CallDeferred(MethodName.DeferredGoToScene, _scenes[idx]);
        }

        public void GoToSelectedLevel(string scenePath)
        {
            if (!_scenes.Contains(scenePath))
            {
                GD.PrintErr($"Scene {scenePath} is not listed as a level scene");
                return;
            }

            CallDeferred(MethodName.DeferredGoToScene, scenePath);
        }

        private void DeferredGoToScene(string path)
        {
            _currentLevel.Free();
            var nextScene = GD.Load<PackedScene>(path);
            _currentLevel = nextScene.Instantiate();
            GetTree().Root.AddChild(_currentLevel);
            GetTree().CurrentScene = _currentLevel;
        }

        public bool HasNextLevel()
        {
            return _scenes.Count > 0 && _scenes.Contains(_currentLevel.SceneFilePath) && _scenes.IndexOf(_currentLevel.SceneFilePath) < _scenes.Count - 1;
        }
    }
}

