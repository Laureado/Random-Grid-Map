using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {

    [SerializeField]
    private int rows = 5;
    [SerializeField]
    private int cols = 8;
    [SerializeField]
    private float tileSize = 1;

    public Sprite[] sprites;
    public InputField lvlNumbers;
    public Text timerText;

    GameObject[] tiles;
    int tileIndex;
    int globalLevel;


    float actualTime;
    float resetTime = 0.2f;

    bool timer = false;

    void Start() {
        tiles = new GameObject[rows * cols];
        GenerateGrid();
    }

    void Update()
    {
        if (timer)
            automatic();
    }

    void automatic()
    {
        if (actualTime <= 0f)
        {
            actualTime = resetTime;
            levelUpLand();
        }
        else
        {
            actualTime -= Time.deltaTime;
        }
    }

    public void ChangeTimer()
    {
        if (timer)
        {
            timerText.text = "Off";
            timer = false;
        }
        else
        {
            timerText.text = "On";
            timer = true;
        }
    }

    private void GenerateGrid()
    {
        GameObject referenceTile = (GameObject)Instantiate(Resources.Load("tiles_0"));

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                tiles[tileIndex] = (GameObject)Instantiate(referenceTile, transform);
                GameObject tile = tiles[tileIndex];

                float posX = col * tileSize;
                float posY = row * -tileSize;

                tile.transform.position = new Vector2(posX, posY);
                tileIndex++;
            }
        }

        Destroy(referenceTile);

        float gridW = cols * tileSize;
        float gridH = rows * tileSize;
        transform.position = new Vector2(-gridW / 2 + tileSize / 2, gridH / 2 - tileSize / 2);
    }

    public void MultipleLevelUp()
    {
        int totalLevelsup = int.Parse(lvlNumbers.text);

        if (!(globalLevel + totalLevelsup > (rows * cols) * (sprites.Length - 1)))
        {
            for (int i = 0; i < totalLevelsup; i++)
            {
                levelUpLand();
            }
        }
    }

    public void nextLevelButton()
    {

        if (lvlNumbers.text == "")
            return;

        if (int.Parse(lvlNumbers.text) > 0)
        {
            MultipleLevelUp();
        }
    }

    public void levelUpLand()
    {
        if (globalLevel >= (rows * cols) * (sprites.Length - 1))
        {
            Debug.Log("Se llego al maximo nivel global: " + globalLevel);
            return;
        }

        int randomTale = Random.Range(0, rows * cols);
        int nextLevel = int.Parse(tiles[randomTale].GetComponent<SpriteRenderer>().sprite.name.Remove(0, 6)) + 1;

        if (nextLevel >= sprites.Length)
        {
            levelUpLand();
        }
        else
        {
            tiles[randomTale].GetComponent<SpriteRenderer>().sprite = sprites[nextLevel];
            globalLevel++;

            if(nextLevel == 2)
            {
                levelBackLand();
            }
        }
    }

    public void levelBackLand()
    {
        

        if (globalLevel < 1)
            return;

        int randomTale = Random.Range(0, rows * cols);
        int returnLevel = int.Parse(tiles[randomTale].GetComponent<SpriteRenderer>().sprite.name.Remove(0, 6)) -1;

        if (returnLevel < 0)
        {
            levelBackLand();
        }
        else
        {
            tiles[randomTale].GetComponent<SpriteRenderer>().sprite = sprites[returnLevel];
            globalLevel--;
        }
    }
}
