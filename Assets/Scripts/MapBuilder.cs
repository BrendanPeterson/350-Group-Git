using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{

    public GameObject[] prefabs;
    public int mapWidth = 30;
    public int mapHeight = 30;
    int buildingFootprint = 10;
    //building footprint creates space between buildings

    // Use this for initialization
    void Start()
    {
        //float seed = Random.Range(0, 1000);
        for (int h = 0; h < mapHeight; h++)
        {
            for (int w = 0; w < mapWidth; w++)
            {
                //int result = (int)(Mathf.PerlinNoise(w/1.0f + seed, h/1.0f + seed) * 16);
                //perlin noise loops through height and width and returns a value between 0 and 1
                //creates groups of buildings based on height
                Vector3 pos = new Vector3(w * buildingFootprint, 0, h * buildingFootprint);
                //Vector3 pos = new Vector3(w, 0, h);
                 int n = Random.Range(0, prefabs.Length);
                 Instantiate(prefabs[n], pos, Quaternion.identity);
                /*if (result < 2)
                    Instantiate(prefabs[1], pos, Quaternion.identity);
                else if (result < 3)
                    Instantiate(prefabs[2], pos, Quaternion.identity);
                else if (result < 4)
                    Instantiate(prefabs[3], pos, Quaternion.identity);
                else if (result < 5)
                    Instantiate(prefabs[4], pos, Quaternion.identity);
                else if (result < 6)
                    Instantiate(prefabs[5], pos, Quaternion.identity);
                else if (result < 7)
                    Instantiate(prefabs[6], pos, Quaternion.identity);
                else if (result < 8)
                    Instantiate(prefabs[7], pos, Quaternion.identity);
                else if (result < 9)
                    Instantiate(prefabs[8], pos, Quaternion.identity);
                else if (result < 10)
                    Instantiate(prefabs[9], pos, Quaternion.identity);
                else if (result < 11)
                    Instantiate(prefabs[10], pos, Quaternion.identity);
                else if (result < 12)
                    Instantiate(prefabs[11], pos, Quaternion.identity);
                else if (result < 13)
                    Instantiate(prefabs[12], pos, Quaternion.identity);
                else if (result < 14)
                    Instantiate(prefabs[13], pos, Quaternion.identity);
                else if (result < 15)
                    Instantiate(prefabs[14], pos, Quaternion.identity);
                else if (result < 16)
                    Instantiate(prefabs[15], pos, Quaternion.identity);*/
            }
        }
    }
}