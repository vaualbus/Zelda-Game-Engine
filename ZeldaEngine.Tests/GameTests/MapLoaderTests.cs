using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using ZeldaEngine.Base.Abstracts.Game;
using ZeldaEngine.Base.Game.ValueObjects;
using ZeldaEngine.Base.Game.ValueObjects.MapLoaderDataTypes;

namespace ZeldaEngine.Tests.GameTests
{
    class MapLoader : IQuestLoader
    {
        public QuestDefinition Load(string fileName)
        {
            using (var sr = new StreamReader(fileName))
                return JsonConvert.DeserializeObject<QuestDefinition>(sr.ReadToEnd());
        }
    }

    [TestFixture]
    public class MapLoaderTests
    {
        [Test]
        public void LoadMapNotThrowException()
        {
            //var mapLoader = new MapLoader();

            //var map = mapLoader.Load("MapsTest/MapTest.json");
            //var testScreen = map.Screens.FirstOrDefault(t => t.Id == 0);

            //var cc = "";
        }
    }
}