using UIScripts;
using UnityEngine;

public class HangarState : GameState
{
    private HangarCameraControl _hangarCameraControl;
    private GameUserInterface _userInterface;
    
    public override void EnterState()
    {
        base.EnterState();

        _hangarCameraControl = HangarCameraControl.instance;
        _userInterface = GameUserInterface.instance;
        
        _hangarCameraControl.EnterHangarMode();
        _userInterface.EnterHangarMode();

        Cursor.lockState = CursorLockMode.None; // TODO - remove
        Cursor.visible = true;
    }

    public override void ExitState()
    {
        base.ExitState();
        
        _hangarCameraControl.ExitHangarMode();
        _userInterface.ExitHangarMode();
        
        Cursor.lockState = CursorLockMode.Locked; // TODO - remove
        Cursor.visible = false;
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        _hangarCameraControl.CameraControl();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }
}
