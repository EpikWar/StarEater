using UIScripts;
using UnityEngine;

public class RTSState : GameState
{
    private GameUserInterface _userInterface;
    
    public override void EnterState()
    {
        base.EnterState();
        
        _userInterface = GameUserInterface.instance;
        
        _userInterface.EnterRTSMode();
    }

    public override void ExitState()
    {
        base.ExitState();
        
       _userInterface.ExitRTSMode(); 
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }
}
