using System;
using System.Reflection;

namespace ZeldaEngine.Base.ValueObjects.Game
{
    public class GameObjectAttachedValues
    {
        public MemberInfo GameObjectNameProperty { get; private set; }

        public MemberInfo AttachedProperty { get; private set; }

        public object CallingGameObject { get; private set; }

        public object CallingScript { get; private set; }

        public GameObjectAttachedValues(MemberInfo gameObjectNameProperty, MemberInfo attachedProperty, object callingGameObject, object callingScript)
        {
            GameObjectNameProperty = gameObjectNameProperty;
            AttachedProperty = attachedProperty;

            CallingGameObject = callingGameObject;
            CallingScript = callingScript;
        }
    }
}