using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoll : MonoBehaviour
{
    [SerializeField] Image dice1;
	[SerializeField] Image dice2;
	public Image playerColor;
	[SerializeField] GameManager gameManager;

	public DiceRoll instance;

	private void Start()
	{
		if (instance != null)
		{
			Debug.LogError("Il y a plus d'une instance de GameManager dans la scène");
			return;
		}

		instance = this;
	}

	public void RollDices()
	{
		var rand1 = UnityEngine.Random.Range(1, 7);
		var rand2 = UnityEngine.Random.Range(1, 7);

		dice1.sprite = gameManager.diceFaces[rand1-1];
		dice2.sprite = gameManager.diceFaces[rand2-1];

		gameManager.diceResult = rand1 + rand2;

		if (gameManager.turn >= gameManager.players.Count * 2)
		{
			gameManager.endTurnButton.SetActive(true);
			GetComponentInChildren<Button>().enabled = false;
			gameManager.NextTurn();
			gameManager.currentPlayerRessources = gameManager.players[(gameManager.startingPlayerIndex + gameManager.turn) % gameManager.players.Count].ressources; 
		}
	}
}
