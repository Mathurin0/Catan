using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BuildingSpot : MonoBehaviour
{
	[SerializeField] GameManager gameManager;
	public List<Tile> adjoiningTiles = new List<Tile>();
	public Image image;
	public bool isCity = false;
	public bool isConstructed = false;

	private void Start()
	{
		image = GetComponent<Image>();
	}
	
	public void OnClick()
	{
		PlayerData player;
		if (gameManager.turn > gameManager.players.Count && gameManager.turn <= gameManager.players.Count * 2)
		{
			player = gameManager.players[(gameManager.startingPlayerIndex + gameManager.players.Count * 2 - gameManager.turn) % gameManager.players.Count];
		}
		else
		{
			player = gameManager.players[(gameManager.startingPlayerIndex + gameManager.turn - 1) % gameManager.players.Count];
		}

		player.victoryPoints++;
		var color = gameObject.GetComponent<Image>().color;
		color.a = 1;
		gameObject.GetComponent<Image>().color = color;

		if (!isConstructed)
		{
			List<Collider2D> touchingColliders = new List<Collider2D>();
			Physics2D.OverlapCollider(gameObject.GetComponent<CircleCollider2D>(), new ContactFilter2D(), touchingColliders);

			foreach (var collider in touchingColliders)
			{
				if (collider.gameObject.CompareTag("Colony"))
				{
					collider.gameObject.SetActive(false);
					gameManager.emptyBuildingSpots.Remove(collider.gameObject);
				}
			}

			isConstructed = true;
			player.buildings.Add(this);
			gameObject.GetComponentInChildren<Button>().enabled = false;

			GetComponent<Button>().enabled = false;
			gameManager.emptyBuildingSpots.Remove(gameObject);

			foreach (var spot in gameManager.emptyBuildingSpots)
			{
				spot.GetComponent<Image>().sprite = gameManager.instance.transparent;
				spot.GetComponentInChildren<Button>().enabled = false;
			}

			if (gameManager.turn <= gameManager.players.Count * 2)
			{
				foreach (var roadSpot in gameManager.emptyRoadSpots)
				{
					if (Vector3.Distance(transform.position, roadSpot.transform.position) <= 60)
					{
						var alpha120 = roadSpot.GetComponent<Image>().color;
						alpha120.a = .5F;
						roadSpot.gameObject.GetComponent<Image>().color = alpha120;
						roadSpot.gameObject.GetComponent<Image>().sprite = player.roadSprite;
						roadSpot.GetComponentInChildren<Button>().enabled = true;
					}
				}
			}

			if (gameManager.turn > gameManager.players.Count * 2)
			{
				player.ressources.Remove(gameManager.ressources[0]);
				player.ressources.Remove(gameManager.ressources[1]);
				player.ressources.Remove(gameManager.ressources[3]);
				player.ressources.Remove(gameManager.ressources[4]);

				if (!player.ressources.Contains(gameManager.ressources[0]) || !player.ressources.Contains(gameManager.ressources[1]) || !player.ressources.Contains(gameManager.ressources[3]) || !player.ressources.Contains(gameManager.ressources[4]))
				{
					gameManager.craftColonyButton.SetActive(false);
				}

				gameManager.players[(gameManager.startingPlayerIndex + gameManager.turn - 1) % gameManager.players.Count].buildings.Add(gameObject.GetComponent<BuildingSpot>());
			}
		}
		else
		{
			player.ressources.Remove(gameManager.ressources[2]);
			player.ressources.Remove(gameManager.ressources[2]);
			player.ressources.Remove(gameManager.ressources[2]);
			player.ressources.Remove(gameManager.ressources[3]);
			player.ressources.Remove(gameManager.ressources[3]);

			var wheatCount = 0;
			var stoneCount = 0;
			foreach (var ressource in player.ressources)
			{
				if (ressource == gameManager.ressources[2])
					stoneCount++;

				if (ressource == gameManager.ressources[3])
					wheatCount++;
			}

			isCity = true;

			if (wheatCount < 2 || stoneCount < 3)
			{
				gameManager.craftColonyButton.SetActive(false);
			}

			foreach (var building in player.buildings)
			{
				if (!building.isCity)
				{
					building.GetComponent<Image>().color = color;
					building.GetComponent<Image>().sprite = player.colonySprite;
				}
			}
		}

		if (player.victoryPoints >= 10)
		{
			gameManager.Victory(player);
		}
	}
}
