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

        public TElement AddElement<TElement>(string name, Action<TElement> action, bool isActive = true) 
            where TElement : class, IUIElement
        {
            if (UIElements.Select(t => t.Value).Any(t => t.ControlName == name))
            {
                _gameEngine.Logger.LogError("Trying adding a game object with the same name");
                return default(TElement);
            }

            var ctorParams = new {_gameEngine};

            var temp = (TElement) Activator.CreateInstance(typeof(TElement), _gameEngine);
            temp.Name = name;

            temp.Width = 300;
            temp.Height = 50;

            action(temp);

            temp.Init();

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

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
            foreach (var uiElement in UIElements.Values)
            {
                uiElement.Element.Update(dt);
            }
        }

        public TElement GetElement<TElement>(string elementName) where TElement : class, IUIElement
        {
            return (TElement) UIElements[elementName].Element;
        }

        public UIElement GetElement(string elementName)
        {
            return(UIElement) UIElements[elementName].Element;
        }
    }
}