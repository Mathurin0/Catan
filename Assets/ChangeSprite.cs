using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image image;
	public Transform textTransform;
	public Sprite baseSprite;
	public Sprite holdSprite;
	public int textTranslation;
    private bool buttonClicked;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!buttonClicked)
		{
			image.sprite = holdSprite;
			textTransform.Translate(new Vector3(0, (-textTranslation), 0));
			buttonClicked = true;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (buttonClicked)
		{
			image.sprite = baseSprite;
			textTransform.Translate(new Vector3(0, (textTranslation), 0));
			buttonClicked = false;
		}
	}
}
