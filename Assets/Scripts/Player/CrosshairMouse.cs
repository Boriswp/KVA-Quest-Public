using UnityEngine;


public class CrosshairMouse : MonoBehaviour
{

    public GameObject player;

    /// <summary>
    /// Vector that goes from player position to crosshair world position.
    /// </summary>
    public static Vector3 AimDirection
    {
        get { return _AimDirection; }
        private set { _AimDirection = value; }
    }
    private static Vector3 _AimDirection;


    public void Update()
    {
        _AimDirection = (MouseInputHandler.MouseWorldPos - player.transform.position).normalized;
        _AimDirection.z = 0f;
    }
}
