using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "PawnsColor", menuName = "PawnsColor")]
public class PlayerData : ScriptableObject
{
	public int id;
	public int victoryPoints;
	public string pawnsColor;
	public string playerName;
	public Sprite roadSprite;
	public Sprite colonySprite;
	public Sprite citySprite;
	public List<RessourceData> ressources = new List<RessourceData>();
	public List<GameObject> roads = new List<GameObject>();
	public List<BuildingSpot> buildings = new List<BuildingSpot>();
}