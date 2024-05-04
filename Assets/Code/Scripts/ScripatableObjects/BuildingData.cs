using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buildings", menuName = "Buildings")]
public class BuildingData : ScriptableObject
{
	public int id;
	public Sprite image;
	public string BuildingName;
	public int victoryPoints;
}
