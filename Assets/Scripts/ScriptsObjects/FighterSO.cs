using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fighter", menuName = "Fighter")]
public class FighterSO : ScriptableObject
{
    public Sprite Avatar;
    public string Name;
    public float Agility;
    public float Force;
    
}