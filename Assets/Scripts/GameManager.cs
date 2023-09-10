using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public PlayableShip[] playablelShip;

    private ShipDataKeeper shipData = new();
    
    public GameState currentGameState { get; private set; }
    private StateMachine _stateMachine = new();
    public RPGState _rpgState = new();
    public RTSState _rtsState = new();
    public HangarState _hangarState = new();

    private void OnEnable()
    {
        #region Singelton

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        #endregion
    }

    private void Awake()
    {
        foreach (PlayableShip ps in playablelShip)
            if (ps.IDShip == shipData.LoadShip().IDShip)
                Instantiate(ps.Ship);
    }

    private void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            _stateMachine.ChangeState(_rtsState);
        }

        if (Input.GetKeyDown("g"))
        {
            _stateMachine.ChangeState(_rpgState);
        }

        _stateMachine.currentState.UpdateState();

        currentGameState = _stateMachine.currentState;
    }

    private void Start()
    {
        _stateMachine.Initialize(SceneManager.GetActiveScene().name == "Fit Hangar" ? _hangarState : _rpgState);
    }

    private void FixedUpdate()
    {
        _stateMachine.currentState.FixedUpdateState();
    }
}

