using System;
using System.Linq.Expressions;
using System.Reflection;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game.Extensions
{
    public static class GameObjectExtensions
    {
        public static TGameObject AttachProperty<TGameObject>(this TGameObject that,
            Expression<Func<TGameObject, object>> goProp,
            Expression<Func<object>> attachProp,
            GameScript callingScript) where TGameObject : class, IGameObject
        {
            //Get expression that associated the property name
            var goPropertyExp = (UnaryExpression) goProp.Body;
            var goPropertyMemberInfo = (MemberExpression) goPropertyExp.Operand;
            var goPropertyMemberRuntimeProperty = goPropertyMemberInfo.Member;

            var attachPropertyExpr = (UnaryExpression) attachProp.Body;
            var attachMemberInfo = (MemberExpression) attachPropertyExpr.Operand;
            var attachMemberRuntimeProperty = attachMemberInfo.Member;

            var decleringAttachPropMemberExpr = ((MemberExpression) goPropertyMemberInfo.Expression);

            that.AttachedValues.Add(new GameObjectAttachedValue(goPropertyMemberRuntimeProperty, attachMemberRuntimeProperty, that, callingScript));

            return that;
        }
    }
}