using UnityEngine;

public class AlienSub : MonoBehaviour
{
    public AlienSubType SubType;
    public AlienSubStatus SubStatus;
    public AlienRace Race;
    public AlienSubWeapon Weapon;
    public float StartSpeed = 1.0f;
    public float Speed;
    public int Health;
    public Vector3 startPoint;
    public Vector3 endPoint;

    void Start()
    {
        transform.position = startPoint;
        transform.LookAt(Vector3.zero);
    }

    void Update()
    {
        if (SubStatus == AlienSubStatus.Destroyed)
        {
            Destroy(gameObject);
            return;
        }

        if (SubStatus == AlienSubStatus.Crashed)
        {
            return;
        }

        if (SubStatus == AlienSubStatus.Landed)
        {
            return;
        }

        if (SubStatus == AlienSubStatus.Moving)
        {
            transform.position = Vector3.RotateTowards(transform.position,
                endPoint,
                Time.deltaTime * Speed * 0.01f,
                0f);
            transform.LookAt(Vector3.zero);
        }
    }
}
