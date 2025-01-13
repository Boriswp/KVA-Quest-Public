using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenShot : MonoBehaviour
{
    [ContextMenu("ScreenShot")]
    public void takeScreenShot()
    {
        ScreenCapture.CaptureScreenshot("/Users/boris/screenshot1.png");
    }
}
