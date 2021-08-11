using Mirror;

public class LevelManager : NetworkBehaviour {
    public Joystick MovementJoystick;
    public Joystick SlapJoystick;

    [ClientRpc]
    public void AllPlayersLoaded() {
        ScreenManager.Instance.SwitchTo("Overlay");
    }
}
