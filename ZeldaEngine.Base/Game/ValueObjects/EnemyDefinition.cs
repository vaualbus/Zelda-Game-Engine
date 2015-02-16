namespace ZeldaEngine.Base.Game.ValueObjects
{
    public class EnemyDefinition
    {
        public string Name { get; private set; }

        public int Demage { get; private set; }

        public int Life { get; private set; }

        public EnemyDefinition(string name, int demage, int life)
        {
            Name = name;
            Demage = demage;
            Life = life;
        }
    }
}