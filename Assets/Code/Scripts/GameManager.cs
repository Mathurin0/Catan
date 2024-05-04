using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] List<Sprite> numbers;
	[SerializeField] List<int> numbersOrder;
	public Sprite transparent;

	[SerializeField] List<TileData> tiles;
	[SerializeField] List<GameObject> tileSpots;
	public List<GameObject> emptyBuildingSpots;
	public List<GameObject> emptyRoadSpots;

	[SerializeField] GameObject board;
	[SerializeField] GameObject startMenu;
	public GameObject diceRoll;
	public GameObject endTurnButton;

	public GameObject victoryPanel;
	public Image winnerColor;
	public TMP_Text firstPlaceText;
	public TMP_Text secondPlaceText;
	public TMP_Text thirdPlaceText;
	public TMP_Text fourthPlaceText;

	public List<Sprite> diceFaces;
	public int diceResult = 0;
	public List<PlayerData> colors;
	public List<PlayerData> players = new List<PlayerData>();
	public int turn = 0;

	public List<RessourceData> ressources;
	public List<GameObject> ressourcesPrefabs;
	public List<RessourceData> currentPlayerRessources;
	[SerializeField] GameObject ressourcesContener;

	public GameObject craftRoadButton;
	public GameObject craftColonyButton;
	public GameObject craftCityButton;
	//[SerializeField] Image CraftdDevelopmentCard;  TODO
	//[SerializeField] Image Trade;

	public int startingPlayerIndex;

	public GameManager instance;


	void Start()
	{
		if (instance != null)
		{
			Debug.LogError("Il y a plus d'une instance de GameManager dans la scène");
			return;
		}

		instance = this;
		LaunchGame();
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void StartGame()
	{
		startMenu.SetActive(false);
		board.SetActive(true);

		LaunchGamePreparation();
	}

	void LaunchGame()
	{
		int i = 0;
		foreach (var tile in tileSpots)
		{
			int randomIndex = UnityEngine.Random.Range(0, tiles.Count);
			tile.GetComponent<Image>().sprite = tiles[randomIndex].sprite;
			tile.GetComponent<Tile>().tile = tiles[randomIndex];
			tile.GetComponent<Tile>().image.sprite = tiles[randomIndex].sprite;

			if (tiles[randomIndex].ressource != null)
			{
				tile.GetComponent<Tile>().numberImage.sprite = numbers[numbersOrder[i]];
				++i;
			}
			else
			{
				tile.GetComponent<Tile>().numberImage.sprite = transparent;
				tile.GetComponentInChildren<Button>().enabled = false;
			}

			tiles.Remove(tiles[randomIndex]);
		}

		ChoosePlayers();
	}

	void ChoosePlayers()
	{
		board.SetActive(false);
		startMenu.SetActive(true);
	}

	void LaunchGamePreparation()
	{
		foreach (var player in players)
		{
			player.roads.Clear();
			player.buildings.Clear();
			player.ressources.Clear();
			player.victoryPoints = 0;
		}
		startingPlayerIndex = 0;

		StartCoroutine(DiceRolls());
	}

	public IEnumerator DiceRolls()
	{
		var firstPlayerIndex = 0;
		var bestDiceResult = 0;

		diceRoll.SetActive(true);
		foreach (var player in players)
		{
			diceRoll.GetComponent<DiceRoll>().playerColor.sprite = player.roadSprite;

			while (diceResult == 0)
			{
				yield return new WaitForChangedResult();
			}

			if (diceResult > bestDiceResult)
			{
				bestDiceResult = diceResult;
				firstPlayerIndex = players.IndexOf(player);
			}

			diceResult = 0;
		}

		diceRoll.GetComponentInChildren<Button>().enabled = false;
		startingPlayerIndex = firstPlayerIndex;
		NextTurn();
	}

	public void NextTurn()
	{
		turn++;

		if (turn <= players.Count * 2)
		{
			FirstColoniesPlacement();
		}
		else
		{
			PlayerTurn(players[(startingPlayerIndex + turn - 1) % players.Count]);
		}
	}

	void DisplayPlayerRessources()
	{
		for (var i = 0; i < ressourcesContener.transform.childCount; i++)
		{
			Destroy(ressourcesContener.transform.GetChild(i).gameObject);
		}

		foreach (var ressource in currentPlayerRessources)
		{
			var ressourceObject = Instantiate(ressourcesPrefabs[ressource.id - 1]);
			ressourceObject.transform.parent = ressourcesContener.transform;
		}
	}

	public void EndTurn()
	{
		endTurnButton.SetActive(false);
		diceRoll.GetComponentInChildren<Button>().enabled = true;
		diceRoll.GetComponent<DiceRoll>().playerColor.sprite = players[(startingPlayerIndex + turn) % players.Count].roadSprite;
	}

	public void FirstColoniesPlacement()
	{
		foreach (var spot in emptyBuildingSpots)
		{
			var color = spot.GetComponent<Image>().color;
			color.a = .5F;
			spot.GetComponent<Image>().color = color;
			spot.GetComponentInChildren<Button>().enabled = true;

			if (turn <= players.Count)
			{
				spot.GetComponent<Image>().sprite = players[(startingPlayerIndex + turn - 1) % players.Count].colonySprite;
			}
			else
			{
				spot.GetComponent<Image>().sprite = players[(startingPlayerIndex + players.Count * 2 - turn) % players.Count].colonySprite;
			}
		}
	}

	public void PlayerTurn(PlayerData player)
	{
		TakeRessources();

		DisplayPlayerRessources();
		
		CraftsAndTrades();
	}

	void TakeRessources()
	{
		foreach (var joueur in players)
		{
			foreach (var building in joueur.buildings)
			{
				foreach (var tile in building.adjoiningTiles)
				{
					if (tile.numberImage.sprite != null && tile.numberImage.sprite == numbers[diceResult])
					{
						joueur.ressources.Add(tile.tile.ressource);

						if (building.isCity)
						{
							joueur.ressources.Add(tile.tile.ressource);
						}

						Debug.Log(joueur.playerName + " a pioché " + tile.tile.ressource.ressourceName);
					}
				}
			}
		}
	}

	void CraftsAndTrades()
	{
		PlayerData player = players[(startingPlayerIndex + turn - 1) % players.Count];

		if (player.ressources.Contains(ressources[0]) && player.ressources.Contains(ressources[4]))
		{
			craftRoadButton.SetActive(true);
		}

		if (!player.ressources.Contains(ressources[0]) || !player.ressources.Contains(ressources[1]) || !player.ressources.Contains(ressources[3]) || !player.ressources.Contains(ressources[4]))
		{
			craftColonyButton.GetComponent<Button>().enabled = false;
		}
		else
		{
			craftColonyButton.SetActive(true);
			craftColonyButton.GetComponent<Button>().enabled = true;
		}

		int wheatCount = 0;
		int stoneCount = 0;

		foreach (var ressource in player.ressources)
		{
			if (ressource == ressources[2])
			{
				stoneCount++;
			}
			else if (ressource == ressources[3])
			{
				wheatCount++;
			}
		}

		if (stoneCount < 2 && wheatCount < 3)
		{
			craftCityButton.GetComponent<Button>().enabled = false;
		}
		else
		{
			craftCityButton.SetActive(true);
			craftCityButton.GetComponent<Button>().enabled = true;
		}
	}

	public void CraftRoad()
	{
		PlayerData player = players[(startingPlayerIndex + turn - 1) % players.Count];

		if (player.ressources.Contains(ressources[0]) && player.ressources.Contains(ressources[4]))
		{
			foreach (var road in player.roads)
			{
				var overlapRoadColliders = new List<Collider2D>();

				if (road != null)
				{
					Physics2D.OverlapCollider(road.GetComponent<Collider2D>(), new ContactFilter2D(), overlapRoadColliders);

					foreach (var collider in overlapRoadColliders)
					{
						if (collider.gameObject.CompareTag("Road") && !collider.gameObject.GetComponent<RoadSpot>().isConstructed)
						{
							var color = collider.gameObject.GetComponent<Image>().color;
							color.a = .5F;
							collider.gameObject.GetComponent<Image>().color = color;
							collider.gameObject.GetComponent<Image>().sprite = player.roadSprite;
							collider.gameObject.GetComponentInChildren<Button>().enabled = true;
						}
					}
				}
			}
		}
	}

	public void CraftColony()
	{
		PlayerData player = players[(startingPlayerIndex + turn - 1) % players.Count];

		if (player.ressources.Contains(ressources[0]) && player.ressources.Contains(ressources[1]) && player.ressources.Contains(ressources[3]) && player.ressources.Contains(ressources[4]))
		{
			foreach (var road in player.roads)
			{
				var overlapRoadColliders = new List<Collider2D>();
				Physics2D.OverlapCollider(road.GetComponent<Collider2D>(), new ContactFilter2D(), overlapRoadColliders);

				foreach (var collider in overlapRoadColliders)
				{
					if (collider.gameObject.CompareTag("Colony") && !collider.gameObject.GetComponent<BuildingSpot>().isConstructed)
					{
						var color = collider.gameObject.GetComponent<Image>().color;
						color.a = .5F;
						collider.gameObject.GetComponent<Image>().color = color;
						collider.gameObject.GetComponent<Image>().sprite = player.colonySprite;
						collider.gameObject.GetComponentInChildren<Button>().enabled = true;
					}
				}
			}
		}
	}

	public void CraftCity()
	{
		PlayerData player = players[(startingPlayerIndex + turn - 1) % players.Count];

		var wheatCount = 0;
		var stoneCount = 0;
		foreach (var ressource in player.ressources)
		{
			if (ressource == ressources[2])
				stoneCount++;

			if (ressource == ressources[3])
				wheatCount++;
		}

		if (wheatCount >= 2 && stoneCount >= 3)
		{
			foreach (var building in player.buildings)
			{
				if (!building.isCity)
				{
					var color = building.GetComponent<Image>().color;
					color.a = .5F;
					building.GetComponent<Image>().color = color;
					building.GetComponent<Image>().sprite = player.citySprite;
					building.GetComponentInChildren<Button>().enabled = true;
				}
			}
		}
	}

	public void Victory(PlayerData winner)
	{
		victoryPanel.SetActive(true);

		PlayerData secondPlayer = null;
		PlayerData thirdPlayer = null;
		PlayerData fourthPlayer = null;
		foreach (var player in players)
		{
			if (player != winner)
			{
				if (secondPlayer == null)
				{
					secondPlayer = player;
				}
				else if (secondPlayer.victoryPoints < player.victoryPoints)
				{
					if (thirdPlayer != null)
					{
						fourthPlayer = thirdPlayer;
					}

					thirdPlayer = secondPlayer;
					secondPlayer = player;
				}
				else if (thirdPlayer == null)
				{
					thirdPlayer = player;
				}
				else if (thirdPlayer.victoryPoints < player.victoryPoints)
				{
					fourthPlayer = thirdPlayer;
					thirdPlayer = player;
				}
				else
				{
					fourthPlayer = player;
				}
			}
		}

		winnerColor.sprite = winner.roadSprite;
		firstPlaceText.text = winner.playerName + " won with " + winner.victoryPoints + " victory points";
		secondPlaceText.text = secondPlayer.playerName + " finished at the second place with " + secondPlayer.victoryPoints + " victory points";
		thirdPlaceText.text = thirdPlayer.playerName + " finished at the third place with " + thirdPlayer.victoryPoints + " victory points";
		
		if (players.Count == 4)
		{
			fourthPlaceText.text = fourthPlayer.playerName + " finished at the last place with " + fourthPlayer.victoryPoints + " victory points";
		}
	}
}
