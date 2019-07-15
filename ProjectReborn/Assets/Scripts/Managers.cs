using UnityEngine;

[RequireComponent(typeof(MenuManager))]
[RequireComponent(typeof(DataManager))]
public class Managers : MonoBehaviour
{
    void Awake()
    {
        Menu = GetComponent<MenuManager>();
        Data = GetComponent<DataManager>();
    }

    public static MenuManager Menu { get; set; }
    public static DataManager Data { get; set; }
}
