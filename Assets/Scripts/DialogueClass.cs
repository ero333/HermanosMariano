using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


public class DialogueClass
{
    public string Speaker;

    public enum PosibleCharacters { CharA, CharB };

    public PosibleCharacters character; 

    [Multiline]
    public string Dialogue;

    public Sprite CharArt;


}
