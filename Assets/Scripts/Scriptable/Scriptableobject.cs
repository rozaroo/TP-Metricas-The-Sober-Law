using UnityEngine;

[CreateAssetMenu(fileName = "scriptable",menuName = "ScriptableObjects",order = 1)]
public class Scriptableobject : ScriptableObject
{
    [SerializeField]
    public GameObject[] Enemies;
}