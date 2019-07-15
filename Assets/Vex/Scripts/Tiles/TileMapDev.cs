using System.Collections;
using UnityEngine;

public class TileMapDev : MonoBehaviour
{
    public Game game;

    public Team team1;
    public Team team2;

    public Player player11;
    public Player player12;
    public Player player21;
    public Player player22;

    private HexTileMap map => game.Map;
    private Ball testBall;

    private void Awake() { }

    private void Start()
    {
        
        game.SetTeamInfo(team1, team1.allPlayers);
        game.SetTeamInfo(team2, team2.allPlayers);

        game.Load();

        //Players will be selected / placed by the player
        for(int i = 0; i < team1.allPlayers.Count; i++)
        {
            team1.allPlayers[i].PlaceOnTile(map.TileAt(i, 1));
        }

        for (int i = 0; i < team2.allPlayers.Count; i++)
        {
            team2.allPlayers[i].PlaceOnTile(map.TileAt(i, -1));
        }

        testBall = game.SpawnBall();

        game.Begin(team1);

        //StartCoroutine(ExampleTurn1());

        //map.DrawLineSeparating(map.TileAt(0, 1), map.TileAt(1, 1));
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            game.CurrentTurn.FocusNextPlayer();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            var p = game.CurrentTurn.CurrentFocusedPlayer;

            testBall.CurrentCarrier.GiveBallTo(p, BallTransferInfo.Type.Initial);

            p.SetUIController("Throw");
        }
    }

    /*
    private IEnumerator ExampleTurn1()
    {
        yield return new WaitForSeconds(2f);

        player11.MoveAction(map.TileAt(5, 1));

        yield return new WaitForSeconds(2f);

        player12.MoveAction(map.TileAt(5, 7));

        yield return new WaitForSeconds(3f);

        //Rejected as player 1 is still moving
        player11.MoveAction(map.TileAt(2, 1));
    }
    */

    /*
    private float time;
    private void Update()
    {
        time += Time.deltaTime;
        TestBallUpdate();
    }

    
    private void TestBall()
    {
        player.Ball = ball;

        player.PlaceOnTile(map.TileAt(1, 1));
        player2.PlaceOnTile(map.TileAt(-1, -1));
    }

    private bool testBallComplete;
    private void TestBallUpdate()
    {
        if(time > 3f && (testBallComplete == false))
        {
            player.Throw(player2);

            testBallComplete = true;
        }
    }

    private void TestPlayer()
    {
        Tile start = map.TileAt(2, 3);
        Tile end = map.TileAt(5, 7);

        player.PlaceOnTile(start);

        player.MoveTo(map, end);
    }

    private void TestPath()
    {
        Tile start = map.TileAt(2, 3);
        Tile end = map.TileAt(5, 7);

        var path = map.GetPath(start, end)?.ValidPath;

        if (path == null)
        {
            return;
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.LogFormat("Current {0} Next {1} Diff {2}", path[i].Position, path[i + 1].Position, path[i + 1].Position - path[1].Position);
        }
    }
    */
}
