using System.Drawing;
using System.Linq;
using Autofac;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.Extensions;
using ZeldaEngine.Base.Game.GameComponents;
using ZeldaEngine.Base.ValueObjects;
using ZeldaEngine.Base.ValueObjects.Game;
using ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes;

namespace ZeldaEngine.Base.Game.GameObjects
{
    public class DrawableGameObject : GameObject
    {
        public string TestString { get; set; }

        public Tile Tile { get; set; }

        public object Color { get; set; }

        public new Rectangle CollisionBounds => new Rectangle((int)Position.X, (int)Position.Y, Tile.Width, Tile.Height);

        public Vector2 Center => new Vector2(Position.X + Tile.Width / 2.0f, Position.Y + Tile.Height / 2.0f);

        public DrawableGameObject(IGameEngine gameEngine)
            : base(gameEngine)
        {
            Tile = new Tile(gameEngine, 0, 0);

            //Default GO Texture
            AddComponent<TextureDrawerComponent>("textureRenderer");
            AddComponent<ColliderGameComponent>("collider");
        }

        public override ObjectType ObjectType
        {
            get { return ObjectType.DrawableObject; }
        }

        protected override void OnUpdate(float dt)
        {
            //foreach (var go in GameObjectFactory.FindNearGameObject(new Vector2(1,1)))
            //{
            //    var playerCenterPoint = new Vector2(Position.X + Texture.Width, Position.Y + Texture.Height);
            //    if (go.Position.Intersect(playerCenterPoint))
            //    {
            //        //We colliding So we can'tmove anymore
            //    }
            //}
        }

        protected override void OnDraw(IRenderEngine renderEngine)
        {
            GetComponent<TextureDrawerComponent>("textureRenderer").Action(this);
        }

        public DrawableGameObject Create(GameObjectDefinition gameObjectDefinition)
        {
            var go = GameEngine.GameObjectFactory.Create<DrawableGameObject>(gameObjectDefinition.Name);
            //go.Texture = GameEngine.TextureData(gameObjectDefinition.Name);
            go.UpdateTypes(gameObjectDefinition.Properties);
            return go;
        }
    }
}