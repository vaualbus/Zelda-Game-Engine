using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public static Dictionary<string, TAttribute> GetAttibutes<TAttribute>(this ScriptableGameObject sGo)
            where TAttribute : Attribute
        {
            var classType = sGo.ScriptManager.CurrentMenagedScript.GetType();
            var returnAttributes = new Dictionary<string, TAttribute>();

            var resultNonPublicFields =
                from t in classType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                let fName = t.Name
                select new
                {
                    FielName = fName,
                    Attribute = (TAttribute)Attribute.GetCustomAttribute(t, typeof(TAttribute))
                };

            var resultNonPublicProps =
                from t in classType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                let fName = t.Name
                select new
                {
                    FielName = fName,
                    Attribute = (TAttribute)Attribute.GetCustomAttribute(t, typeof(TAttribute))
                };

            foreach (var el in resultNonPublicFields.Where(t => t.Attribute != null))
                returnAttributes.Add(el.FielName, el.Attribute);

            foreach (var el in resultNonPublicProps.Where(t => t.Attribute != null))
                returnAttributes.Add(el.FielName, el.Attribute);


            return returnAttributes;
        }
    }
}