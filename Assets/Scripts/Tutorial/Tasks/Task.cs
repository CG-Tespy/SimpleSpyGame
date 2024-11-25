using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class Task : MonoBehaviour
{
    public string taskName;
    //public TMP_Text text;
    public Text text;
    [TextAreaAttribute] public string taskText;
    [HideInInspector] public Tutorial tutorial;
    public abstract bool Done();
}
