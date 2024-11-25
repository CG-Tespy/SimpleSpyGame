using UnityEngine;
using TMPro;

public abstract class Task : MonoBehaviour
{
    public string taskName;
    public TMP_Text text;
    public Tutorial tutorial;
    public abstract bool Done();
}
