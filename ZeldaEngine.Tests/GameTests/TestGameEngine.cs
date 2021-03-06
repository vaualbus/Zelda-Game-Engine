﻿using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects;

namespace ZeldaEngine.Tests.GameTests
{
    public class TestGameEngine : IGameEngine
    {
        public GameConfig GameConfig
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IInputManager InputManager
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IRenderEngine RenderEngine
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IScriptEngine ScriptEngine
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IAudioEngine AudioEngine
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public IContentLoader ResourceLoader
        {
            get { throw new System.NotImplementedException(); }
        }

        public ILogger Logger
        {
            get { throw new System.NotImplementedException(); }
        }

        public IGameObjectFactory GameObjectFactory
        {
            get; set;
        }

        public IResourceData Texture2DData(string assetName)
        {
            throw new System.NotImplementedException();
        }

        public void ExitGame()
        {
            throw new System.NotImplementedException();
        }
    }
}