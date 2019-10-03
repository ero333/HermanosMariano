using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]


public class DialogueClass
{
    public Sprite background;
    public string Speaker;

    public enum PosibleCharacters { CharA, CharB };

    public PosibleCharacters character; 

    [TextArea]
    public string Dialogue;

    public Sprite CharArt;


}
