using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameSetup : MonoBehaviour
{

    GameObject[,] worldGrid;
    int[,] nextGenCells;
    int gridsize;
    //GameObject cell;
    float lastCheck;
    bool run;

    // Start is called before the first frame update
    void Start()
    {
        gridsize = 25;     
        run = false;
        nextGenCells = new int[gridsize,gridsize];
        createGrid();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {                
                Debug.Log(hit.collider.gameObject.name + " is " + hit.collider.gameObject.tag);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            //calculateNextGeneration();

            //updateNextGen();

            if (run)
            {
                run = false;
            }
            else
            {
                run = true;
            }
        }

        if (run)
        {
            calculateNextGeneration();
            updateNextGen();
        }
        
    }

    private void updateNextGen()
    {
        //i live celle med mindre en 2 i live naboer dør
        //i live celle med 2-3 i live naboer overlever        
        //i live celle med 4 eller flere i live naboer dør
        //død celle med præcis 3 naboer genopliver

        for (int i = 0; i < gridsize; i++)
        {
            for (int j = 0; j < gridsize; j++)            {
                GameObject cell = worldGrid[i, j];
                MeshRenderer cellRender = cell.GetComponent<MeshRenderer>();

                switch (nextGenCells[i,j])
                {
                    case 0:
                    case 1:
                        if (cell.tag.StartsWith("ali"))
                        {
                            cellRender.material.color = Color.red;
                            cell.tag = "dead";
                            Debug.Log("Death by lonelines: " + cell.name);
                            break;
                        }
                        break;

                    case 2:
                        if (cell.tag.StartsWith("d"))
                        {
                            break;
                        }
                        else
                        {
                            cellRender.material.color = Color.green;
                            cell.tag = "alive";
                            break;
                        }                        
                    case 3:
                        if (cell.tag.StartsWith("d"))
                        {
                            cellRender.material.color = Color.green;
                            cell.tag = "alive";
                        }
                        break;
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        if (cell.tag == "alive")
                        {
                            Debug.Log("Overpopulation! " + cell.name);
                            cellRender.material.color = Color.red;
                            cell.tag = "dead";
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }    

    void calculateNextGeneration() 
    {
        for (int i = 0; i < gridsize; i++)
        {
            for (int j = 0; j < gridsize; j++)
            {
                GameObject cell = worldGrid[i, j];
                //Debug.Log("doing cell " + cell.name);
                string[] XYcoord = cell.name.Split(':');

                
                int adjecantCells = getAdjacentCells(int.Parse(XYcoord[0]), int.Parse(XYcoord[1]));                
                
                nextGenCells[i, j] = adjecantCells;

 
                    //Debug.Log(cell.name + " got " + adjecantCells + " adjecant cells");
                
                
            }
        }
    }

    /// <summary>
    /// Counts adjacent (3x3=9 total) live cells based around origin cell
    /// </summary>
    /// <param name="x">X coord</param>
    /// <param name="y">Y coord</param>
    /// <returns>Number of adjecant cells, minus itself if live</returns>
    int getAdjacentCells(int x, int y)
    {
        int adjacent = 0;
        int checkX, checkY;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                checkX = i + x;
                checkY = j + y;

                try
                {
                    //If coords are inside worldbounds and contains a live cell then count as neighbour
                    if (checkBounds(checkX) && checkBounds(checkY) && worldGrid[checkX, checkY].tag == "alive")
                    {
                        adjacent++;
                    }

                    //Checks if a number is within the worldgrid
                    bool checkBounds(int check)
                    {
                        if (check < gridsize && check >= 0)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.Message);
                    throw;
                }
            }
        }

        //If coordinate where the search originated contains a live cell, minus 1 since we dont count ourselves as a neighbour
        if (worldGrid[x,y].tag == "alive")
        {
            adjacent--;
        }
        return adjacent;
    }

    void createGrid()
    {
        worldGrid = new GameObject[gridsize, gridsize];

        for (int i = 0; i < gridsize; i++)
        {
            for (int j = 0; j < gridsize; j++)
            {
                worldGrid[i, j] = renderCell(i, j, false);
            }
        }

        GameObject renderCell(int x, int y, bool living)
        {
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
            g.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            g.transform.position = new Vector3(x + 1, 0, y + 1);
            g.name = x + ":" + y;
            g.AddComponent<BoxCollider>();
            g.AddComponent<cellClick>();

            if (living)
            {
                g.GetComponent<MeshRenderer>().material.color = Color.green;
                g.tag = "alive";
            }
            else
            {
                g.GetComponent<MeshRenderer>().material.color = Color.red;
                g.tag = "dead";
            }

            return g;
        }


    }



}