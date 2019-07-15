using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum Stage
    {
        Creating,  //not yet loaded
        SetUp, //deployment
        Play,  //users taking turns etc.
        Result //post completion, not yet unloaded 
    }

    [Serializable]
    public class TeamInfo
    {
        public Team team;
        public List<Player> playersInGame;
        //to do: substitues
        public int score;
    }

    public static Game Current { get; private set; }

    public Stage CurrentStage { get; private set; }

    [SerializeField] Turn turnPrefab;
    [SerializeField] HexTileMap mapPrefab;
    [SerializeField] Ball ballPrefab;
    [SerializeField] List<Vector2Int> ballSpawnPoints;
    [SerializeField] Goal goalPrefab;
    [SerializeField] List<Vector2Int> goalSpawnPoints;

    public HexTileMap Map { get; private set; }

    public Turn CurrentTurn { get; private set; }
    private TeamInfo currentTeamInfo;
    private List<Ball> balls = new List<Ball>();
    private List<TeamInfo> teams = new List<TeamInfo>();

    private void Awake()
    {
        //Temp
        Current = this;
    }

    public void SetTeamInfo(Team team, List<Player> playersInGame)
    {
        if (CurrentStage != Stage.Creating)
        {
            Debug.LogWarning("Game: Cannot add team once Game has begun");
            return;
        }

        teams.Add(new TeamInfo()
        {
            team = team,
            playersInGame = playersInGame,
            score = 0
        });
    }

    public TeamInfo GetInfo(Team team)
    {
        return teams.FirstOrDefault(x => team == x.team);
    }

    //Transitition from Creating to SetUp
    public void Load()
    {
        if (CurrentStage != Stage.Creating)
        {
            Debug.LogWarning("Game: Cannot load once Game has begun");
            return;
        }

        if (teams == null || teams.Count < 2)
        {
            Debug.LogWarning("Game: Cannot load with fewer than two teams");
            return;
        }

        Map = Instantiate(mapPrefab);
        Map.Create();
    
        foreach(Vector2Int goalPos in goalSpawnPoints)
        {
            Goal goal = Instantiate(goalPrefab);
            goal.PlaceOnTile(Map.TileAt(goalPos));
        }

        CurrentStage = Stage.SetUp;
    }

    //Transitition from SetUp to Playing
    public void Begin(Team teamToPlayFirst = null)
    {
        if (CurrentStage != Stage.SetUp)
        {
            Debug.LogWarning("Game: Cannot begin if not in setup");
            return;
        }

        CurrentStage = Stage.Play;

        BeginTurn(GetInfo(teamToPlayFirst) ?? teams.RandomValue());
    }

    public Ball SpawnBall(Tile tile = null)
    {
        if (CurrentStage == Stage.SetUp || CurrentStage == Stage.Play)
        {
            var b = Instantiate(ballPrefab);

            tile = tile ?? Map.TileAt(ballSpawnPoints[(int)(UnityEngine.Random.value * ballSpawnPoints.Count)]);
            tile.ReceiveBall(new BallTransferInfo()
            {
                ball = b,
                type = BallTransferInfo.Type.Initial
            });

            balls.Add(b);

            return b;
        }

        return null;
    }

    public void BeginTurn(TeamInfo t)
    {
        if(CurrentStage != Stage.Play)
        {
            return;
        }

        currentTeamInfo = t;
        CurrentTurn = Instantiate(turnPrefab);

        CurrentTurn.Begin(t.team, t.playersInGame);

        CurrentTurn.OnTurnEnd += TurnEnded;
    }

    public void TurnEnded()
    {
        Destroy(CurrentTurn.gameObject);

        BeginTurn(teams.Next(currentTeamInfo));
    }

    public void Score(Team team, int value, Ball ball)
    {
        GetInfo(team).score += value;

        balls.Remove(ball);
        Destroy(ball.gameObject);

        SpawnBall();
    }
}
