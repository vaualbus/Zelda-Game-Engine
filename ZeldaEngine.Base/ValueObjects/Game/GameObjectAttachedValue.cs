using System;
using System.Reflection;

namespace ZeldaEngine.Base.ValueObjects.Game
{
    public class GameObjectAttachedValue
    {
        public MemberInfo GameObjectNameProperty { get; private set; }

        public MemberInfo AttachedProperty { get; private set; }

        public object CallingObject { get; private set; }

        public object CallingScript { get; private set; }

        public GameObjectAttachedValue(MemberInfo gameObjectNameProperty, MemberInfo attachedProperty, object callingObject, object callingScript)
        {
            GameObjectNameProperty = gameObjectNameProperty;
            AttachedProperty = attachedProperty;

            CallingObject = callingObject;
            CallingScript = callingScript;
        }
    }
}