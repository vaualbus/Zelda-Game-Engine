using System.CodeDom;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.Extensions;
using ZeldaEngine.Base.Game.ValueObjects;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;

namespace ZeldaEngine.Base.Game.GameObjects
{
    public class EnemyGameObject : MovableGameObject
    {
        public EnemySpawnLocation SpawnLocation { get; set; }

        public override ObjectType ObjectType
        {
            get { return ObjectType.Enemy; }
        }

        public int Demage { get; set; }

        public int Life { get; set; }

        public float WalkVelocity { get; set; }

        public EnemyGameObject(IGameEngine gameEngine)
            : base(gameEngine)
        {
        }

        protected override void OnUpdate(float dt)
        {
        }

        protected override void OnDraw(IRenderEngine renderEngine)
        {
            //Need to call this so the textureRendererC Component Action is called.
            base.OnDraw(renderEngine);

            var cc = "";
        }

        public EnemyGameObject Create(EnemyDefinition enemyDefinition)
        {
            var enemy = GameObjectFactory.Create<EnemyGameObject>(
                enemyDefinition.Name,
                t =>
                {
                    t.Demage = enemyDefinition.Demage;
                    t.Life = enemyDefinition.Life;
                });

            return enemy;
        }

        public void UpdateEnemy(EnemyGameObject go, ScreenEnemyDefinition enemyDescription)
        {
            go.SpawnLocation = enemyDescription.SpawnLocation;
            go.UpdateTypes(enemyDescription.Properties);
        }
    }
}