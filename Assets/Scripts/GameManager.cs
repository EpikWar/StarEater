using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public PlayableShip[] playablelShip;
    
    private readonly StateMachine _stateMachine = new();
    private readonly RPGState _rpgState = new();
    private readonly RTSState _rtsState = new();
    private readonly HangarState _hangarState = new();
    
    private ShipDataKeeper shipData = new();

    private GameState currentGameState;


    private void Awake()
    {
        //Spawn Player ship
        foreach (var ps in playablelShip) 
            if (ps.IDShip == shipData.LoadShip().IDShip) {
                Instantiate(ps.Ship);
                break;
            }
    }

    private void OnEnable()
    {
    #region Singelton

        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;

    #endregion
    }
    
    private void Start()
    {
        _stateMachine.Initialize(SceneManager.GetActiveScene().name == "Fit Hangar" ? _hangarState : _rpgState);
    }

    private void Update()
    {
        _stateMachine.currentState.UpdateState();
        
        currentGameState = _stateMachine.currentState;
        
        //Temporally State change
        if (Input.GetKeyDown("t"))
            _stateMachine.ChangeState(_rtsState);
        if (Input.GetKeyDown("g"))
            _stateMachine.ChangeState(_rpgState);
    }

    private void FixedUpdate()
    {
        _stateMachine.currentState.FixedUpdateState();
    }

#region State check

    public bool IsRPGState()
    {
        return currentGameState == _rpgState;
    }

    public bool IsRTSState()
    {
        return currentGameState == _rtsState;
    }

    public bool IsHangarState()
    {
        return currentGameState == _hangarState;
    }

#endregion
    
}










