using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty",menuName = "Difficulty",order = 1)]
public class Difficulty : ScriptableObject
{
    public float Normal;
    public float Easy;
    public float VeryEasy;
}