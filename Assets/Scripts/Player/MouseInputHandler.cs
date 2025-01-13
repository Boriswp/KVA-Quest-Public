using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputHandler : MonoBehaviour
{
    public static bool IsMouseActive { get { return _isMouseActive; } private set { _isMouseActive = value; } }
    private static bool _isMouseActive;

    /// <summary>
    /// Mouse input from (-1,-1) to (1,1).
    /// </summary>
    public static Vector3 MouseInput { get { return _MouseInput; } private set { _MouseInput = value; } }
    private static Vector3 _MouseInput;

    /// <summary>
    /// Mouse position in pixel coordinates.
    /// </summary>
    public static Vector3 MousePixelPos { get { return _MousePixelPos; } private set { _MousePixelPos = value; } }
    private static Vector3 _MousePixelPos;

    /// <summary>
    /// Mouse position in world coordinates.
    /// </summary>
    public static Vector3 MouseWorldPos { get { return _MouseWorldPos; } private set { _MouseWorldPos = value; } }
    private static Vector3 _MouseWorldPos;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (_MouseInput.sqrMagnitude > 0)
            if (!_isMouseActive)
                _isMouseActive = true;


        _MouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (_MouseInput.magnitude > 1)
            _MouseInput *= (100f / _MouseInput.magnitude) / 100f;

        _MousePixelPos = Input.mousePosition;
        _MousePixelPos.z = 20f;
        _MouseWorldPos = _camera.ScreenToWorldPoint(_MousePixelPos);
        _MouseWorldPos.z = 0f;

    }
}
