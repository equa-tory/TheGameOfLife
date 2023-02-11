using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int SCREEN_WIDTH = 86;
    public static int SCREEN_HEIGTH = 48;

    [Range(0, 1)]
    public float speed = 0.1f;

    private float timer = 0;

    Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGTH];

    public Cell cell;

    private void Start()
    {
        PlaceCell(2);
    }

    private void Update()
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
        if (type == 1)
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
