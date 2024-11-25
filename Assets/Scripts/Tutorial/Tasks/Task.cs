using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Task : MonoBehaviour
{
    public string taskName;
    public TMP_Text text;
    public abstract bool Done();
}
