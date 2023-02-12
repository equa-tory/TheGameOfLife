using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;
using System.Xml.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Hud hud;
    
    public static int SCREEN_WIDTH = 86;
    public static int SCREEN_HEIGTH = 48;

    [Range(0, 1)]
    public float speed = 0.1f;

    private float timer = 0;

    public bool simalationEnabled = false;

    Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGTH];

    public Cell cell;

    public GameObject helpPanel;

    public TMP_Text speedText;
    public Image speedBG;
    float speedBgA;
    float speedA;
    float _timer;

    public float timeToFade;

    [Header("Keybindings")]
    public KeyCode playKey = KeyCode.Space;
    public KeyCode stopKey = KeyCode.Space;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;

    private void Start()
    {
        EventManager.StartListening("SavePattern", SavePattern);
        EventManager.StartListening("LoadPattern", LoadPattern);

        Instance = this;
        PlaceCell(0);
    }

    private void Update()
    {
        if (simalationEnabled)
        {
            if (timer >= speed)
            {
                timer = 0f;

                CountNeighbors();

                PopulationControl();
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        UserInput();
    }

    void UserInput()
    {
        if (!hud.isActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                int x = Mathf.RoundToInt(mousePoint.x);
                int y = Mathf.RoundToInt(mousePoint.y);

                if (x >= 0 && y >= 0 && x < SCREEN_WIDTH && y < SCREEN_HEIGTH)
                {
                    grid[x, y].SetAlive(!grid[x, y].isAlive);
                }
            }

            if (Input.GetKeyUp(stopKey))
            {
                if (simalationEnabled) simalationEnabled = false;
                else simalationEnabled = true;
                helpPanel.SetActive(false);
            }

            /*if (Input.GetKeyUp(playKey))
            {
                simalationEnabled = true;
            }*/

            if (Input.GetKeyUp(saveKey))
            {
                //SavePattern();
                hud.ShowSaveDialog();
                helpPanel.SetActive(false);
            }

            if (Input.GetKeyUp(loadKey))
            {
                //LoadPattern();
                hud.ShowLoadDialog();
                helpPanel.SetActive(false);
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (speed < 1) speed += 0.05f;
                else speed = 1;

                speedBgA = .5f;
                speedA = 1f;
                _timer = timeToFade;

                speedText.text = "Speed: " + System.Math.Round(speed, 2);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                if (speed > 0.01) speed -= 0.05f;
                else speed = 0.01f;

                speedBgA = .5f;
                speedA = 1f;
                _timer = timeToFade;

                speedText.text = "Speed: " + System.Math.Round(speed, 2);
            }

            speedBG.color = new Color(0, 0, 0, speedBgA);
            speedText.color = new Color(255, 255, 255, speedA);

            if (speedBgA > 0)
            {
                if (_timer > 0) _timer -= Time.deltaTime;
                else if(_timer<=0)speedBgA -= Time.deltaTime / 2;
            }

            if (speedA > 0)
            {
                if (_timer > 0) _timer -= Time.deltaTime;
                else if (_timer <= 0) speedA -= Time.deltaTime;
            }
        }

    }

    private void LoadPattern()
    {
        simalationEnabled = false;
        helpPanel.SetActive(true);

        string path = "patterns";

        if (!Directory.Exists(path)) return;

        XmlSerializer serializer = new XmlSerializer(typeof(Pattern));
        string patternName = hud.loadDialog.patternName.options[hud.loadDialog.patternName.value].text;
        path = path + "/" + patternName + ".xml";


        StreamReader reader = new StreamReader(path);
        Pattern pattern = (Pattern)serializer.Deserialize(reader.BaseStream);
        reader.Close();

        bool isAlive;

        int x = 0, y = 0;

        Debug.Log(pattern.patternString);

        foreach(char c in pattern.patternString)
        {
            if(c.ToString() == "1")
            {
                isAlive = true;
            }
            else
            {
                isAlive = false;
            }

            grid[x, y].SetAlive(isAlive);

            x++;

            if(x == SCREEN_WIDTH)
            {
                x = 0;
                y++;
            }
        }
    }

    private void SavePattern()
    {
        string path = "patterns";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        Pattern pattern = new Pattern();

        string patternString = null;

        for(int y = 0; y<SCREEN_HEIGTH; y++)
        {
            for(int x = 0; x<SCREEN_WIDTH; x++)
            {
                if(grid[x,y].isAlive == false)
                {
                    patternString += "0";
                }
                else
                {
                    patternString += "1";
                }
            }
        }

        pattern.patternString = patternString;

        XmlSerializer serializer = new XmlSerializer(typeof(Pattern));

        StreamWriter writer = new StreamWriter(path + "/" + hud.saveDialog.patternName.text + ".xml");
        serializer.Serialize(writer.BaseStream, pattern);
        writer.Close();

        Debug.Log(pattern.patternString);
    }


    void CountNeighbors()
    {
        for (int y = 0; y < SCREEN_HEIGTH; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                int numNeighbors = 0;

                if (y + 1 < SCREEN_HEIGTH)
                {
                    if (grid[x, y + 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                if (x + 1 < SCREEN_WIDTH)
                {
                    if (grid[x + 1, y].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (y - 1 >= 0)
                {
                    if (grid[x, y - 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                if (x - 1 >= 0)
                {
                    if (grid[x - 1, y].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (x + 1 < SCREEN_WIDTH && y + 1 < SCREEN_HEIGTH)
                {
                    if (grid[x + 1, y + 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (x - 1 >= 0 && y + 1 < SCREEN_HEIGTH)
                {
                    if (grid[x - 1, y + 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (x + 1 < SCREEN_WIDTH && y - 1 >= 0)
                {
                    if (grid[x + 1, y - 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                if (x - 1 >= 0 && y - 1 >= 0)
                {
                    if (grid[x - 1, y - 1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                grid[x, y].numNeighbors = numNeighbors;

            }
        }
    }

    void PopulationControl()
    {
        for (int y = 0; y < SCREEN_HEIGTH; y++)
        {
            for(int x = 0; x < SCREEN_WIDTH; x++)
            {


                if (grid[x, y].isAlive)
                {
                    if (grid[x, y].numNeighbors != 2 && grid[x, y].numNeighbors != 3)
                    {
                        grid[x, y].SetAlive(false);
                    }
                }
                else
                {
                    if(grid[x,y].numNeighbors == 3)
                    {
                        grid[x, y].SetAlive(true);
                    }
                }
            }
        }
    }

    void PlaceCell(int type)
    {
        if (type == 0)
        {

            for (int y = 0; y < SCREEN_HEIGTH; y++)
            {
                for (int x = 0; x < SCREEN_WIDTH; x++)
                {
                    Cell _cell = Instantiate(cell, new Vector2(x, y), Quaternion.identity);
                    grid[x, y] = _cell;
                    grid[x, y].SetAlive(false);
                }

            }
        }
        else if (type == 1)
        {


            for (int y = 0; y < SCREEN_HEIGTH; y++)
            {
                for (int x = 0; x < SCREEN_WIDTH; x++)
                {
                    Cell _cell = Instantiate(cell, new Vector2(x, y), Quaternion.identity);
                    grid[x, y] = _cell;
                    grid[x, y].SetAlive(RandAliveCell());
                }

            }
        }
        else if(type == 2)
        {
            for(int y = 0; y < SCREEN_HEIGTH; y++)
            {
                for(int x = 0; x < SCREEN_WIDTH; x++)
                {
                    Cell _cell = Instantiate(cell, new Vector2(x, y), Quaternion.identity);
                    grid[x, y] = _cell;
                    grid[x, y].SetAlive(false);
                }
            }

            for(int y = 21; y < 24; y++)
            {
                for(int x = 31; x < 38; x++)
                {
                    if (x != 34)
                    {
                        if (y == 21 || y == 23)
                        {
                            grid[x, y].SetAlive(true);
                        } else if (y == 22 && ((x != 22) && (x != 36))){
                            grid[x, y].SetAlive(true);
                        }
                    }
                }
            }
        }
    }

    bool RandAliveCell()
    {
        int r = Random.Range(0, 100);

        if (r > 75)
        {
            return true;
        }

        return false;
    }
}
