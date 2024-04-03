using UIScripts;

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
    }

    public override void ExitState()
    {
        base.ExitState();

        _hangarCameraControl.ExitHangarMode();
        _userInterface.ExitHangarMode();
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
