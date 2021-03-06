﻿using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes;

namespace ZeldaEngine.Base.Game.GameObjects
{
    public class ItemGameObject : GameObject, IItem
    {
        public ItemGameObject(IGameEngine gameEngine) 
            : base(gameEngine)
        {
        }

        public ItemGameObject Create(ItemDefinition itemDefinition)
        {
            return null;
        }
    }
}