using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Notes", order = 51)]
public class NoteData : ScriptableObject {

    [SerializeField]
    private string title;

    [SerializeField]
    private string description;

    public string Title
    {
        get
        {
            return title;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
    }
}
