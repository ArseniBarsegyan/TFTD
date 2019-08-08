using UnityEngine;

[RequireComponent(typeof(MenuManager))]
[RequireComponent(typeof(DataManager))]
[RequireComponent(typeof(SettingsManager))]
[RequireComponent(typeof(MissionManager))]
public class Managers : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Menu = GetComponent<MenuManager>();
        Data = GetComponent<DataManager>();
        Settings = GetComponent<SettingsManager>();
        Mission = GetComponent<MissionManager>();
    }

    public static MenuManager Menu { get; set; }
    public static DataManager Data { get; set; }
    public static SettingsManager Settings { get; set; }
    public static MissionManager Mission { get; set; }
}
