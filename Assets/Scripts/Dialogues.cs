﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogues
{
    public Sprite chefsImage;
    public string name;
    [TextArea(3, 10)]
    public string initialDialogue;
    [TextArea(3, 10)]
    public string completeDialogue;

}
