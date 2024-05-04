using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Ressource", menuName = "Ressources/Ressource")]
public class RessourceData : ScriptableObject
{
    public int id;
    public Sprite image;
    public string ressourceName;
}
