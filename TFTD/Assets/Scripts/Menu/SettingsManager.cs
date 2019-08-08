using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private int _prefferedRefreshRate = 60;
    public FullScreenMode ScreenMode { get; private set; } = FullScreenMode.FullScreenWindow;

    public void ChangeResolution(int width, int height)
    {
        Screen.SetResolution(width, height, ScreenMode, _prefferedRefreshRate);
    }

    public void ChangeScreenMode(FullScreenMode screenMode)
    {
        ScreenMode = screenMode;
    }
}
