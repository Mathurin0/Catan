using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Tile", menuName = "Ressources/Tile")]
public class TileData : ScriptableObject
{
	public int id;
	public Sprite sprite;
	public string TileName;
	public RessourceData ressource;
}
