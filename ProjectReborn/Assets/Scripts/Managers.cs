using UnityEngine;

[RequireComponent(typeof(MenuManager))]
public class Managers : MonoBehaviour
{
    void Awake()
    {
        Menu = GetComponent<MenuManager>();
    }

    public static MenuManager Menu { get; set; }
}
