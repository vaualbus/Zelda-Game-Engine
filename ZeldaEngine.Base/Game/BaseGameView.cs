﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game
{
    public abstract class BaseGameView : IGameView, IEquatable<BaseGameView>
    {      
        public int ScreenId { get; set; }

        public Vector2 PlayerStartPosition { get; private set; }

        public virtual string Name { get; private set; }
       
        public Vector2 ScreenPosition { get; set; }

        public ColorPalette ColorPalette { get; set; }

        public WarpPoint[] WarpPoints { get; set; }

        public List<IGameObject> GameObjects { get; private set; }

        public List<EnemyGameObject> Enemies
        {
            get { return GameObjects.OfType<EnemyGameObject>().ToList(); }
        }

        public List<DrawableGameObject> DrawableGameObjects
        {
            get { return GameObjects.OfType<DrawableGameObject>().ToList(); }
        }

        public List<ScriptableGameObject> Scripts { get; private set; }

        protected BaseGameView(string screenName, Vector2 screenPosition, Vector2 playerStartPosition = null)
        {
            Name = screenName;
            ScreenPosition = screenPosition;
            Scripts = new List<ScriptableGameObject>();
            ScreenId = 0;
            PlayerStartPosition = playerStartPosition;
        
            GameObjects = new List<IGameObject>();
        }

        public void Draw(IRenderEngine renderEngine)
        {
            foreach (var gameObject in GameObjects)
            {
                if (gameObject is ScriptableGameObject)
                    (gameObject as ScriptableGameObject)?.Draw(renderEngine);

                else
                    gameObject.Draw(renderEngine);
            }
            OnDraw(renderEngine);
        }

        public void Update(float dt)
        {
            foreach (var gameObject in GameObjects)
            {
                if (gameObject is ScriptableGameObject)
                    (gameObject as ScriptableGameObject)?.Update(dt);
                else
                    gameObject.Update(dt);
            }
            OnUpdate(dt);
        }

        public virtual void OnUpdate(float dt) { }

        public virtual void OnDraw(IRenderEngine renderEngine) { }

        public void Dispose()
        {
            foreach (var script in Scripts)
                script.Dispose();
        }

        public bool Equals(BaseGameView other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ScreenId.Equals(other.ScreenId) && string.Equals(Name, other.Name) && Equals(WarpPoints, other.WarpPoints);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseGameView) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ScreenId.GetHashCode();
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (WarpPoints != null ? WarpPoints.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(BaseGameView left, BaseGameView right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BaseGameView left, BaseGameView right)
        {
            return !Equals(left, right);
        }
    }
}