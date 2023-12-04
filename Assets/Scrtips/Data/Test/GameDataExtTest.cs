using NUnit.Framework;
using UnityEngine.AddressableAssets;

public class GameDataExtTest 
{
    private GameData gameData;

    [SetUp]
    public void Setup()
    {
        gameData = Addressables.LoadAssetAsync<GameData>("Assets/Prefab/gamedata.asset").WaitForCompletion();
        gameData.Init();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        gameData.Release();
        Addressables.Release(gameData);
    }

    [Test]
    public void GetCurLvTableTest()
    {
        var actual = gameData.lvTable.GetCurLvTable(1, 10);
        if (actual == null)
        {
            Assert.Fail("1, 10 Find Table Fail"); 
        }
        Assert.AreEqual(1, actual.startLv);


    }



    [Test]
    public void CalcIncrStatTest()
    {

        var actual= gameData.lvTable.CalcIncrStat(1, 2, 1).Item1;
        var expected = 1;
        Assert.AreEqual(expected,actual);

        actual = gameData.lvTable.CalcIncrStat(1, 3, 1).Item1;
        expected = 2;
        Assert.AreEqual(expected, actual);

        actual = gameData.lvTable.CalcIncrStat(1, 11, 1).Item1;
        expected = 10;
        Assert.AreEqual(expected, actual);

        actual = gameData.lvTable.CalcIncrStat(1, 101, 1).Item1;
        expected = 550;
        Assert.AreEqual(expected, actual);

        actual = gameData.lvTable.CalcIncrStat(1, 40, 30).Item1;
        expected = 39;
        Assert.AreEqual(expected, actual);
        // Assert
        // Assert.AreEqual(5, result);
    }
}
