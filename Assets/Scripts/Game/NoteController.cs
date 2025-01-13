using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : InteractiveObject
{
    public NoteData data;

    public override void Action()
    {
        var note = NoteUIController.Instance;
        note.title.text = data.Title;
        note.description.text = data.Description;
        note.gameObject.SetActive(true);
    }
}
