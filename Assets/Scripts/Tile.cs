using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Tile
{
    private bool error;

    private int posX;

    private int posY;

    public int value;

    private bool isChangeable;

    private int isHighlighted;

    private bool isSelected;

    private Button button;

    private Text text;

    public List<GameObject> suggestionText;

    private Font font;

    private Image sprite;

    public GameObject tileObject;

    private Color32 grey = new Color32(0x72, 0x72, 0x72, 0xFF);

    private Color32 red = new Color32(0xDA, 0x00, 0x37, 0xFF);

    private Color32 redChangable = new Color32(0x9A, 0x03, 0x1E, 0xFF);

    // private Color32 highlightColor = new Color32(0x85, 0xCD, 0xFD, 0xFF);

    // private Color32 highlightColor = new Color32(0xB4, 0xCD, 0xE6, 0xFF);

    private Color32 highlightColor = new Color32(0xA0, 0xBF, 0xE0, 0xFF);

    private Color32 highlightAlpha = new Color32(0x78, 0x95, 0xCB, 0xFF);

    private Color32 notHighlight = new Color32(0xC5, 0xDF, 0xF8, 0xFF);

    private Color32 black = new Color32(0x22, 0x22, 0x22, 0xFF);

    public Tile(int posX, int posY, int value, bool isChangeable, Sprite texture, Font font, GameObject canvas)
    {
        this.posX = posX;

        this.posY = posY;

        this.isChangeable = isChangeable;

        this.value = value;

        this.font = font;

        isSelected = false;

        error = false;


        tileObject = new GameObject();

        tileObject.name = "Tile" + posX + posY;

        tileObject.transform.position = new Vector3(posX * 100 - 400, posY * 100 - 190);


        GameObject textObject = new GameObject();

        text = textObject.transform.AddComponent<Text>();

        text.name = "TileText";

        text.resizeTextForBestFit = true;

        text.resizeTextMaxSize = 100;

        text.alignment = TextAnchor.MiddleCenter;

        text.font = font;

        text.color = Color.black;

        if (value != -1)
        {
            text.text = value.ToString();
        }

        suggestionText = new List<GameObject>();

        textObject.transform.SetParent(tileObject.transform, false);

        for (int index = 0; index < 9; index++)
        {
            suggestionText.Add(new GameObject());

            if (index == 0)
            {
                suggestionText[index].transform.position = new Vector3(-27, -27);
            }
            else if (index == 1)
            {
                suggestionText[index].transform.position = new Vector3(-27, 0);
            }
            else if (index == 2)
            {
                suggestionText[index].transform.position = new Vector3(-27, 27);
            }
            else if (index == 3)
            {
                suggestionText[index].transform.position = new Vector3(0, -27);
            }
            else if (index == 4)
            {
                suggestionText[index].transform.position = new Vector3(0, 0);
            }
            else if (index == 5)
            {
                suggestionText[index].transform.position = new Vector3(0, 27);
            }
            else if (index == 6)
            {
                suggestionText[index].transform.position = new Vector3(27, -27);
            }
            else if (index == 7)
            {
                suggestionText[index].transform.position = new Vector3(27, 0);
            }
            else if (index == 8)
            {
                suggestionText[index].transform.position = new Vector3(27, 27);
            }

            Text textAux = suggestionText[index].transform.AddComponent<Text>();

            textAux.name = "Suggestion" + (index + 1).ToString();

            textAux.rectTransform.sizeDelta = new Vector2(27, 27);

            textAux.resizeTextForBestFit = true;

            textAux.resizeTextMaxSize = 100;

            textAux.alignment = TextAnchor.MiddleCenter;

            textAux.font = font;

            textAux.color = Color.black;

            textAux.text = (index + 1).ToString();

            suggestionText[index].gameObject.SetActive(false);

            suggestionText[index].transform.SetParent(tileObject.transform, false);
        }


        sprite = tileObject.transform.AddComponent<Image>();

        sprite.sprite = texture;

        sprite.color = notHighlight;

        sprite.transform.SetParent(textObject.transform, false);


        tileObject.transform.SetParent(canvas.transform, false);
    }

    public void SetValue(int value)
    {
        if (isChangeable)
        {
            this.value = value;

            if (value != -1)
            {
                text.text = value.ToString();
            }
            else
            {
                text.text = "";
            }
        }
    }

    public int GetPosX()
    {
        return posX;
    }

    public int GetPosY()
    {
        return posY;
    }

    public bool GetIsChangeable()
    {
        return isChangeable;
    }

    public void SetError(bool error)
    {
        this.error = error;

        UpdateColor();
    }

    public bool GetError()
    {
        return error;
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;

        UpdateColor();
    }

    public void SetHighlighted(int highlighted)
    {
        isHighlighted = highlighted;

        UpdateColor();
    }

    public void UpdateColor()
    {
        if (error)
        {
            if (isChangeable)
            {
                text.color = redChangable;
            }
            else
            {
                text.color = red;
            }
        }
        else
        {
            if (isSelected)
            {
                if (isChangeable)
                {
                    text.color = black;
                }
                else
                {
                    text.color = black;
                }
            }
            else
            {
                if (isChangeable)
                {
                    text.color = grey;
                }
                else
                {
                    text.color = black;
                }
            }
        }

        if (isHighlighted == 0)
        {
            sprite.color = notHighlight;
        }
        else
        if (isHighlighted == 1)
        {
            sprite.color = highlightColor;
        }
        else
        if (isHighlighted == 2)
        {
            sprite.color = highlightAlpha;
        }
    }
}
