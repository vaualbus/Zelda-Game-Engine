﻿using System;
using System.Collections.Generic;
using System.Linq;
using ZeldaEngine.Base;
using ZeldaEngine.Base.Abstracts;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Abstracts.ScriptEngine;
using ZeldaEngine.Base.ValueObjects;


namespace ZeldaEngine.ScriptEngine
{
    public class GameScriptParmaterProvider : IScriptParamaterProvider
    {
        private readonly Dictionary<GameScript, object[]> _paramaters;
        private readonly IScriptEngine _engine;
        private readonly ILogger _logger;

        public GameScriptParmaterProvider(IScriptEngine engine, ILogger logger)
        {
            _paramaters = new Dictionary<GameScript, object[]>();
            _engine = engine;
            _logger = logger;

            _logger.LogInfo("Init script Paramater");
        }

        public bool AddParamater(string scriptName, IEnumerable<object> @params)
        {
            var gameScript = _engine.ScriptRepository.GetScript(scriptName);
            if (_paramaters.ContainsKey(gameScript))
                return false;

            if (@params == null)
            {
                _paramaters.Add(gameScript, new object[] {});
                return false;
            }

            _paramaters.Add(gameScript, @params.ToArray());
            return true;
        }

        public void ChangeParamaters(string scriptName, IEnumerable<object> newParams)
        {
            var gameScript = _engine.ScriptRepository.GetScript(scriptName);
            _paramaters[gameScript] = newParams.ToArray();           
        }

        public void RemoveParamaters(string scriptName)
        {
           _paramaters.Remove(_engine.ScriptRepository.GetScript(scriptName));
        }

        public object[] GetParamatersForScript(string scriptName)
        {
            return _paramaters.ContainsKey(_engine.ScriptRepository.GetScript(scriptName))
                ? _paramaters[_engine.ScriptRepository.GetScript(scriptName)]
                : new object[] {};
        }

        public object[] GetParamatersForScript(GameScript gameScript)
        {
            return  _paramaters.ContainsKey(gameScript) ? _paramaters[gameScript] : null;
        }

        public bool AddParamater(GameScript gameScript, object[] @params)
        {
            if (_paramaters.ContainsKey(gameScript))
                return false;

            if (@params == null)
            {
                _paramaters.Add(gameScript, new object[] { });
                _logger.LogInfo("Adding null value to {0}", gameScript.ToString());
                return false;
            }

            _paramaters.Add(gameScript, @params.ToArray());
            return true;
        }
    }
}