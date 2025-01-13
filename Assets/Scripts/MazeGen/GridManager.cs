using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    public int columnLenght;
    public int rowLenght;
    public float x_Space;
    public float y_Space;
    public float x_Start;
    public float y_Start;
    public GameObject cellPrefab;
    public static GridManager instance;
    void Create()
    {
        for (int i = 0; i < columnLenght * rowLenght; i++)
        {
            Instantiate(cellPrefab, new Vector3(x_Start + (x_Space * (i % columnLenght)), y_Start + (-y_Space * (i / columnLenght))), Quaternion.Euler(270, 0, 0), transform);
        }
    }
    [MenuItem("CONTEXT/MonoBehaviour/Create")]
    static void Gen()
    {
        instance.Create();
    }
}
#endif
