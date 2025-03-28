using BoardTools;
using Bombs;
using GameModes;
using System.Collections;
using UnityEngine;

public delegate void CallBackFunction();

public class GameManagerScript : MonoBehaviour {

    public PrefabsList PrefabsList;

    public UI UI;
    public ShipMovementScript Movement;

    public static GameManagerScript Instance;

    void Start()
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        Instance = this;

        SetApplicationParameters();
        InitializeScripts();

        //Global.Initialize();

        Phases.Initialize();
        Rules.Initialize();
        Board.Initialize();
        yield return Roster.Initialize();
        Selection.Initialize();
        BombsManager.Initialize();
        ActionsHolder.Initialize();
        Combat.Initialize();
        Triggers.Initialize();
        yield return DamageDecks.Initialize();

        GameMode.CurrentGameMode.StartBattle();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI.ToggleInGameMenu();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!Console.IsActive) UI.GoNextShortcut();
        }

        if (Phases.CurrentSubPhase != null) Phases.CurrentSubPhase.Update();

        //TestShotDistance();
    }

    private void SetApplicationParameters()
    {
        Application.targetFrameRate = 60;

        QualitySettings.SetQualityLevel(Options.Quality);
        if (Options.ShowFps) GameObject.Find("UI/PlayersPanel").transform.Find("FpsHolder").gameObject.SetActive(true);

        Options.UpdateVolume();
    }

    private void InitializeScripts()
    {
        PrefabsList = this.GetComponent<PrefabsList>();
        UI = this.GetComponent<UI>();
        Movement = this.GetComponent<ShipMovementScript>();
    }

    public static void Wait(float seconds, CallBackFunction callBack)
    {
        Instance.StartCoroutine(Instance.WaitCoroutine(seconds, callBack));
    }

    IEnumerator WaitCoroutine(float seconds, CallBackFunction callBack)
    {
        yield return new WaitForSeconds(seconds);
        callBack();
    }

    private void TestShotDistance()
    {
        Ship.GenericShip ship1 = Roster.GetShipById("ShipId:1");
        Ship.GenericShip ship2 = Roster.GetShipById("ShipId:2");
        ShotInfo shotInfo = new ShotInfo(ship1, ship2, ship1.PrimaryWeapons);
        if (shotInfo.IsShotAvailable) MovementTemplates.ShowRangeRuler(shotInfo.MinDistance); else MovementTemplates.ReturnRangeRuler();
    }

}
