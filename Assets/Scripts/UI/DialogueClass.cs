using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueClass
{
    public enum PosibleCharacters { CharA, CharB };
    public PosibleCharacters character;

    public string Speaker;

    public Sprite CharArt;

    [TextArea]
    public string Dialogue;
    
    public Sprite background;
}
