using UnityEngine;
using UnityEngine.UI;

public abstract class Task : MonoBehaviour
{
    public string taskName;
    [TextArea] public string taskText;
    [HideInInspector] public Tutorial tutorial;
    public abstract bool Done();
}
