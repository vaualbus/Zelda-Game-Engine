namespace ZeldaEngine.Base.ValueObjects.MapLoaderDataTypes
{
    public class ItemDefinition
    {
        public string Name { get; private set; }

        public string ItemTextureName { get; private set; }

        public int DemageCount { get; private set; }

        public int DefenseCount { get; private set; }

        public ItemDefinition(string name, string itemTextureName, int demageCount, int defenseCount = 0)
        {
            Name = name;
            ItemTextureName = itemTextureName;
            DemageCount = demageCount;
            DefenseCount = defenseCount;
        }
    }
}