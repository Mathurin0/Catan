using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Ressources/Recipe")]
public class RecipeData : ScriptableObject
{
	public int id;
    public List<RessourceData> CostRessources;
	public BuildingData Crafted;
}
