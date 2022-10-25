using System.Collections.Generic;
using System.IO;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    
    private static string _levelStatsMapFIleName;
    
    private const string PlayerSpriteIndexKey = "PlayerSpriteIndex";
    private const string PassedLevelsKey = "PassedLevels";
    
    public static List<Sprite> TeamSprites = new();
    public static List<LevelStats> LevelStatsMap = new();

    private static LevelsConfig _levelsConfig;
    
    public static int CurrentLevelIndex;
    
    public static int TeamId = 1;

    public static bool IsNetwork;

    public static NetworkPlayerController PlayerController;

    public static int LevelsCount => _levelsConfig.LevelsData.Count;
    
    public static int PlayerSpriteIndex
    {
        get => PlayerPrefs.GetInt(PlayerSpriteIndexKey, 1);
        set
        {
            TeamSprites = new List<Sprite>(_levelsConfig.TeamSprites);
            (TeamSprites[1], TeamSprites[value]) = (TeamSprites[value], TeamSprites[1]);
            PlayerPrefs.SetInt(PlayerSpriteIndexKey, value);
        }
    }

    public static int PassedLevelsCount
    {
        get => PlayerPrefs.GetInt(PassedLevelsKey, 0);
        set => PlayerPrefs.SetInt(PassedLevelsKey, value);
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
        _levelStatsMapFIleName = Path.Combine(Application.persistentDataPath, "LevelStatsMap");
        
        _levelsConfig = Resources.Load<LevelsConfig>("Configs/LevelsConfig");
        LevelStatsMap = Storage.Load<List<LevelStats>>(_levelStatsMapFIleName);
        
        TeamSprites = new List<Sprite>(_levelsConfig.TeamSprites);
        (TeamSprites[1], TeamSprites[PlayerSpriteIndex]) = (TeamSprites[PlayerSpriteIndex], TeamSprites[1]);
    }

    public static LevelData GetLevel(int index)
    {
        return _levelsConfig.LevelsData[index];
    }
    
    public static LevelData GenerateLevel(int playerCount)
    {
        var levelData = new LevelData();
        var usedPositions = new Dictionary<Vector3, float>();

        for (var i = 1; i <= playerCount; i++)
        {
            var node = GenerateRandomNode(i, ref usedPositions);
            levelData.NodesData.Add(node);
        }

        var neutralNodesCount = Random.Range(4, 8);
        for (var i = 0; i < neutralNodesCount; i++)
        {
            var n= GenerateRandomNode(0, ref usedPositions);
            levelData.NodesData.Add(n);
        }

        return levelData;
    }

    public static void CompleteLevel(bool isWin)
    {
        var levelPassTime = PageManager.Get<GameplayPage>().LevelPassTime;
        var levelPassTimeInSeconds = (int) levelPassTime.TimeOfDay.TotalSeconds;
        var reward = 0;
        
        if (!isWin)
        {
            reward = FundsManager.CalculateReward(CurrentLevelIndex + 1, levelPassTimeInSeconds, false, false, false);
            FundsManager.MakeTransaction(reward);
            PopupManager.Open<MatchCompletionPopup>(new MatchCompletionPopup.Param(false, false, levelPassTime,
                reward));
            return;
        }

        var isFirstWin = CurrentLevelIndex > LevelStatsMap.Count - 1;
        var isNewBestTime = !isFirstWin && levelPassTime < LevelStatsMap[CurrentLevelIndex].BestTime;
        
        if (isFirstWin)
        {
            var levelStats = new LevelStats { BestTime = levelPassTime };
            LevelStatsMap.Add(levelStats);
        }
        else if (isNewBestTime)
        {
            LevelStatsMap[CurrentLevelIndex].BestTime = levelPassTime;
        }
        
        SaveLevelStatsMap();

        reward = FundsManager.CalculateReward(CurrentLevelIndex + 1, levelPassTimeInSeconds, true,
            isFirstWin, isFirstWin || isNewBestTime);
        PopupManager.Open<MatchCompletionPopup>(new MatchCompletionPopup.Param(true, isFirstWin || isNewBestTime,
            levelPassTime, reward));

        FundsManager.MakeTransaction(reward);
    }

    public static void CompleteNetworkLevel(bool isWin)
    {
        var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        
        var levelPassTime = PlayerController.LevelPassTime;
        var levelPassTimeInSeconds = (int) levelPassTime.TimeOfDay.TotalSeconds;
        var reward = 0;
        
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
        {
        }
        
        if (!isWin)
        {
            reward = FundsManager.CalculateNetworkReward(playerCount, levelPassTimeInSeconds, false);
            FundsManager.MakeTransaction(reward);
            PopupManager.Open<MatchCompletionPopup>(new MatchCompletionPopup.Param(false, false, levelPassTime,
                reward));
            return;
        }

        reward = FundsManager.CalculateNetworkReward(playerCount, levelPassTimeInSeconds, true);
        PopupManager.Open<MatchCompletionPopup>(new MatchCompletionPopup.Param(true, false, levelPassTime, reward));

        FundsManager.MakeTransaction(reward);
    }

    private static NodeData GenerateRandomNode(int teamId, ref Dictionary<Vector3, float> usedPositions)
    {
        var node = new NodeData
        {
            TeamId = teamId,
            Position = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f)),
            Radius = Random.Range(30, 50)
        };

        while (usedPositions.Any(p => 
            p.Value + node.Radius + 20 >= Camera.main.WorldToScreenPoint(node.Position - p.Key).magnitude))
        {
            node.Position = new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f));
            node.Radius = Random.Range(30, 50);
        }
            
        node.MaxUnitCount = (int) (node.Radius * 2);
        node.Injection = node.MaxUnitCount / 2;
        usedPositions.Add(node.Position, node.Radius);

        return node;
    }

    private static void SaveLevelStatsMap()
    {
        Storage.Save(_levelStatsMapFIleName, LevelStatsMap);
    }
}