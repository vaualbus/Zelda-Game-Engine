using System.Collections.Generic;
using System.Linq;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.GameObjects;

namespace ZeldaEngine.Base.Game.Extensions
{
    public static class TypeExtensions
    {
        public static IGameObject UpdateTypes(this IGameObject that, Dictionary<string, object> properties)
        {
            var goType = that.GetType();

            foreach (var key in properties.Select(t => new { PropName = t.Key, PropValue = t.Value }))
            {
                var propType = goType.GetProperty(key.PropName);
                if (propType != null)
                {
                    if (propType.GetMethod.ReturnType != key.PropValue.GetType())
                        continue;

                    propType.SetValue(that, key.PropValue);
                }
            }

            return that;
        }

        public static EnemyGameObject UpdateTypes(this EnemyGameObject that, Dictionary<string, object> properties)
        {
            var goType = that.GetType();

            foreach (var key in properties.Select(t => new { PropName = t.Key, PropValue = t.Value }))
            {
                var propType = goType.GetProperty(key.PropName);
                if (propType != null)
                {
                    if (propType.GetMethod.ReturnType != key.PropValue.GetType())
                        continue;

                    propType.SetValue(that, key.PropValue);
                }
            }

            return that;
        }
    }
}