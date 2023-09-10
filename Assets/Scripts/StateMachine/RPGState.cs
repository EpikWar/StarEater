using UIScripts;
using UnityEngine;

public class RPGState : GameState 
{
    private RPGPlayerControl _rpgPlayerControl;
    private RPGCameraControl _rpgCameraControl;
    private GameUserInterface _userInterface;
    
    public override void EnterState()
    {
        base.EnterState();
        
        _rpgPlayerControl = RPGPlayerControl.instance;
        _rpgCameraControl = RPGCameraControl.instance;
        _userInterface = GameUserInterface.instance;
        
        _rpgCameraControl.EnterRPGMode();
        _userInterface.EnterRPGMode();
    }
    
    public override void ExitState()
    {
        base.ExitState();
        
        _rpgCameraControl.ExitRPGMode();
        _userInterface.ExitRPGMode();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        _rpgPlayerControl.PlayerRotation();
        _rpgCameraControl.CameraControl();
        
        _userInterface.RPGVirtualCursor();
        _userInterface.LockTarget();
        _userInterface.ShipInfoControl();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        
        _rpgPlayerControl.PlayerMovement();
    }
    
}
