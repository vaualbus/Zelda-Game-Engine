using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.Toolkit.Graphics;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Game.Abstracts;
using ZeldaEngine.Game.ValueObjects;

namespace ZeldaEngine.Game.UI.GameObjects
{
    public class UIGameObject : DrawableGameObject
    {
        private readonly IGameEngine _gameEngine;

        public Dictionary<string, IUIContext> UIElements { get; } 

        public string UIGoupName { get; }

        public UIGameObject(IGameEngine gameEngine, string uiGroupName)
            : base(gameEngine)
        {
            _gameEngine = gameEngine;
            UIGoupName = uiGroupName;
            UIElements = new Dictionary<string, IUIContext>();
        }

        public TElement AddElement<TElement>(string name, Action<IUIElement> action, bool isActive = true) 
            where TElement : class, IUIElement
        {
            if (UIElements.Select(t => t.Value).Any(t => t.ControlName == name))
            {
                _gameEngine.Logger.LogError("Trying adding a game object with the same name");
                return default(TElement);
            }

            var temp = (TElement) Activator.CreateInstance(typeof(TElement));
            temp.Name = name;
            temp.GameEngine = _gameEngine;

            temp.Width = 300;
            temp.Height = 50;

            action(temp);

            var uiContext = new UIContext(name, isActive ? UIState.Active : UIState.Disabled, temp);

            UIElements.Add(name, uiContext);

            return temp;
        }

        protected override void OnDraw(IRenderEngine renderEngine)
        {
            //Call it first, it apply th texture render component
            base.OnDraw(renderEngine);
            foreach (var uiElement in UIElements.Values)
            {
                uiElement.Element.Draw(renderEngine);
            }
        }
    }
}