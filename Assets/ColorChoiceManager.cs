using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorChoiceManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
	public TMP_Text playerName;
	List<ColorChoiceManager> colorChoiceManagers = new List<ColorChoiceManager>();
	int selected = 0;
	public PlayerData player;


	void Start()
	{
		for (var i = 0; i < 4; i++)
		{
			colorChoiceManagers.Add(this.transform.parent.parent.GetChild(i).GetComponentInChildren<ColorChoiceManager>());
		}
	}

    public void SelectBlue()
	{
		if (selected != 1)
		{
			if (selected != 0)
			{
				ReloadManagers(selected, true);
			}

			selected = 1;
			player = gameManager.colors[selected - 1];
			ReloadManagers(1, false);
		}
		else
		{
			selected = 0;
			player = null;
			ReloadManagers(1, true);
		}
	}

	public void SelectRed()
	{
		if (selected != 2)
		{
			if (selected != 0)
			{
				ReloadManagers(selected, true);
			}

			selected = 2;
			player = gameManager.colors[selected - 1];
			ReloadManagers(2, false);
		}
		else
		{
			selected = 0;
			player = null;
			ReloadManagers(2, true);
		}
	}

	public void SelectYellow()
	{
		if (selected != 3)
		{
			if (selected != 0)
			{
				ReloadManagers(selected, true);
			}

			selected = 3;
			player = gameManager.colors[selected - 1];
			ReloadManagers(3, false);
		}
		else
		{
			selected = 0;
			player = null;
			ReloadManagers(3, true);
		}
	}

	public void SelectWhite()
	{
		if (selected != 4)
		{
			if (selected != 0)
			{
				ReloadManagers(selected, true);
			}

			selected = 4;
			player = gameManager.colors[selected - 1];
			ReloadManagers(4, false);
		}
		else
		{
			selected = 0;
			player = null;
			ReloadManagers(4, true);
		}
	}

	public void ValidatePlayers()
	{
		foreach (var colorManager in colorChoiceManagers)
		{
			if (colorManager.player != null && colorManager.playerName.text.Length > 1)
			{
				colorManager.player.playerName = colorManager.playerName.text;
				gameManager.players.Add(colorManager.player);
			}
		}

		if (gameManager.players.Count >= 3)
		{
			gameManager.StartGame();
		}
	}

	void ReloadManagers(int colorId, bool isAlreadySelected)
	{
		foreach (var colorManager in colorChoiceManagers)
		{
			if (colorManager != this)
			{
				colorManager.gameObject.transform.GetChild(colorId-1).gameObject.SetActive(isAlreadySelected);
			}
			else if (isAlreadySelected)
			{
				colorManager.gameObject.transform.GetChild(colorId - 1).gameObject.SetActive(true);
			}
		}
	}
}
