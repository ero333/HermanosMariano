using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene", menuName = "ScriptableObjects/Cutscene", order = 1)]
public class DialogueData : ScriptableObject
{
    public Sprite charAstart;
    public Sprite charBstart;

    public DialogueClass[] Dialogues;
}
