using System;

namespace ZeldaEngine.ScriptEngine.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EnemyScript : Attribute
    {
        public uint EnemyId { get; set; }

        public EnemyScript(uint enemyId)
        {
            EnemyId = enemyId;
        }
    }
}
