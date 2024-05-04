using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadSpot : MonoBehaviour
{
	[SerializeField] GameManager gameManager;
	public Image image;
	public bool isConstructed = false;

	private void Start()
	{
		image = GetComponent<Image>();
	}

	public void OnClick()
	{
		var color = gameObject.GetComponent<Image>().color;
		color.a = 1;
		gameObject.GetComponent<Image>().color = color;
		gameObject.GetComponent<Image>().sprite = gameManager.players[(gameManager.startingPlayerIndex + gameManager.turn - 1) % gameManager.players.Count].roadSprite;

		isConstructed = true;

		GetComponent<Button>().enabled = false;
		gameManager.emptyRoadSpots.Remove(gameObject);

		foreach (var spot in gameManager.emptyRoadSpots)
		{
			spot.GetComponent<Image>().sprite = gameManager.instance.transparent;
		}

		if (gameManager.turn > gameManager.players.Count && gameManager.turn <= gameManager.players.Count * 2 + 1)
		{
			gameManager.players[(gameManager.startingPlayerIndex + gameManager.players.Count * 2 - gameManager.turn) % gameManager.players.Count].roads.Add(gameObject);

			gameObject.GetComponent<Image>().sprite = gameManager.players[(gameManager.startingPlayerIndex + gameManager.players.Count * 2 - gameManager.turn) % gameManager.players.Count].roadSprite;
		}
		if (gameManager.turn <= gameManager.players.Count)
		{
			gameManager.players[(gameManager.startingPlayerIndex + gameManager.turn - 1) % gameManager.players.Count].roads.Add(gameObject);
		}
		
		if (gameManager.turn > gameManager.players.Count *2)
		{
			gameManager.players[(gameManager.startingPlayerIndex + gameManager.turn - 1) % gameManager.players.Count].ressources.Remove(gameManager.ressources[0]);
			gameManager.players[(gameManager.startingPlayerIndex + gameManager.turn - 1) % gameManager.players.Count].ressources.Remove(gameManager.ressources[4]);

			if (!gameManager.players[(gameManager.startingPlayerIndex + gameManager.turn - 1) % gameManager.players.Count].ressources.Contains(gameManager.ressources[0]) || !gameManager.players[(gameManager.startingPlayerIndex + gameManager.turn - 1) % gameManager.players.Count].ressources.Contains(gameManager.ressources[4]))
			{
				gameManager.craftRoadButton.SetActive(false);
			}

			gameManager.players[(gameManager.startingPlayerIndex + gameManager.turn - 1) % gameManager.players.Count].roads.Add(gameObject);
		}

		if (gameManager.turn < gameManager.players.Count * 2)
		{
			gameManager.NextTurn();
		}
		else if (gameManager.turn == gameManager.players.Count * 2)
		{
			gameManager.diceRoll.GetComponentInChildren<Button>().enabled = true;
			gameManager.diceRoll.GetComponent<DiceRoll>().playerColor.sprite = gameManager.players[gameManager.startingPlayerIndex].roadSprite;
			gameManager.endTurnButton.SetActive(true);
		}
	}
}
