using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    // Object Declarations
    public GameObject terrainObject;
    public GameObject waterObject;
    public GameObject playerObject;

    public GameObject EnemyLocations;
    public GameObject SmallObjectLocations;
    public GameObject LargeObjectLocations;

    public Transform EnemyMarker;
    public Transform SmallMarker;
    public Transform LargeMarker;

    // User interface elements
    public GameObject InfoPanel;

    private float averageHeight;

	// Use this for initialization
	void Start()
    {
        ClosePanel();
	}
	
	// Update is called once per frame
	void Update()
    {
		
	}
    
    public void ClosePanel()
    {
        InfoPanel.gameObject.SetActive(false);
    }

    public void GenerateLevel()
    {
        int width = 250;
        int height = 250;
        float[] displaceMap = new float[width * height];
        int[] areaMap = new int[width * height];

        // Clear object lists
        foreach (Transform child in EnemyLocations.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in SmallObjectLocations.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in LargeObjectLocations.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        // Perlin Noise
        PerlinNoise(displaceMap, width, height);

        // Random Walk
        RandomWalk(areaMap);

        // Combination
        MapCombination(displaceMap, areaMap, width, height);

        // Location Generation
        SmallObjects(areaMap, width, height);
        LargeObjects();
        GenerateEnemyLocations(areaMap, width, height);
        GeneratePlayerLocation(areaMap, width, height);
    }

    void PerlinNoise(float[] dM, int w, int h)
    {
        System.Random rand = new System.Random();
        float[] permutations = new float[w * h];

        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                float randomValue = rand.Next(0, 256);
                permutations[j * w + i] = randomValue / 256.0f;
            }
        }

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float noise = 0.0f;
                float scale = 1.0f;
                float scaleAcc = 0.0f;

                for (int i = 0; i < 4; i++)
                {
                    // Distance between significant pixels
                    int pitch = w >> 4;

                    // Calculate significant pixel locations
                    int x1 = (x / pitch) * pitch;
                    int y1 = (y / pitch) * pitch;

                    int x2 = (x1 + pitch) % w;
                    int y2 = (y1 + pitch) % w;

                    // Calculate weightings based on distance from significant pixels
                    float blendX = (float)(x - x1) / (float)pitch;
                    float blendY = (float)(y - y1) / (float)pitch;

                    // Calculate top and bottom samples using horizontal weighting
                    float sampleT = (1.0f - blendX) * permutations[y1 * w + x1] + blendX * permutations[y1 * w + x2];
                    float sampleB = (1.0f - blendX) * permutations[y2 * w + x1] + blendX * permutations[y2 * w + x2];

                    // Increase accumulative scalar
                    scaleAcc += scale;

                    // Calculate noise value using samples and vertical weighting
                    noise += (blendY * (sampleB - sampleT) + sampleT) * scale;
                    scale = scale / 0.5f;
                }

                // Scale to acceptable range
                dM[y * w + x] = noise / scaleAcc;
            }
        }

        for (int y = 0; y < w; y++)
        {
            for (int x = 0; x < h; x++)
            {
                float noise = 0.0f;
                float scale = 1.0f;
                float scaleAcc = 0.0f;

                for (int i = 0; i < 1; i++)
                {
                    int pitch = w >> 1;
                    int x1 = (x / pitch) * pitch;
                    int y1 = (y / pitch) * pitch;

                    int x2 = (x1 + pitch) % w;
                    int y2 = (y1 + pitch) % w;

                    float blendX = (float)(x - x1) / (float)pitch;
                    float blendY = (float)(y - y1) / (float)pitch;

                    float sampleT = (1.0f - blendX) * permutations[y1 * w + x1] + blendX * permutations[y1 * w + x2];
                    float sampleB = (1.0f - blendX) * permutations[y2 * w + x1] + blendX * permutations[y2 * w + x2];

                    scaleAcc += scale;
                    noise += (blendY * (sampleB - sampleT) + sampleT) * scale;
                    scale = scale / 0.15f;
                }

                // Scale to seed range
                dM[y * w + x] *= noise / scaleAcc;
            }
        }
    }

    void RandomWalk(int[] aM)
    {
        // One eighth scale of original map
        int resolution = 64;
        int[] walkMap = new int[64 * 64];
        System.Random rand = new System.Random();

	    for (int j = 0; j < resolution; j++)
	    {
		    for (int i = 0; i < resolution; i++)
		    {
			    walkMap[j * resolution + i] = 1;
		    }
	    }

	    int xVal = 31; int yVal = 1;
        int count = 0;
        bool finished = false;
	
	// Loop until finished
	    do
	    {
	    	// Change value of current gridsquare to zero
	    	if (walkMap[yVal * resolution + xVal] == 1)
	    	{
	    		walkMap[yVal * resolution + xVal] = 0;
	    		count++;
	    	}
	    	
	    	// Choose one of four directions
	    	int direction = rand.Next(0, 4);

	    	// Check not too close to edge
	    	switch (direction)
	    	{
	    	case 0:								// East
	    		if (xVal<resolution - 1)
	    		{
	    			xVal++;
	    		}
	    		break;
	    	case 1:								// South
	    		if (yVal<resolution - 1)
	    		{
	    			yVal++;
	    		}
	    		break;
	    	case 2:								// West
	    		if (xVal > 1)
	    		{
	    			xVal--;
	    		}
	    		break;
	    	case 3:								// North
	    		if (yVal > 1)
	    		{
	    			yVal--;
	    		}
	    		break;
	    	}

	    	// If within two squares of edge
	    	if (xVal< 2 || xVal> resolution - 2 || yVal< 2 || yVal> resolution - 2)
	    	{
	    		// If at least one third is walkable
	    		if (count > (resolution* resolution) / 3)
	    		{
	    			// This ends the loop
	    			finished = true;
	    		}
	    	}
	    } while (!finished);
	    
	    // Loop prevents edges being part of the playing area
	    for (int j = 0; j<resolution; j++)
	    {
	    	for (int i = 0; i<resolution; i++)
	    	{
	    		if (i == 0 || i == resolution - 1 || j == 0 || j == resolution - 1)
	    		{
	    			walkMap[j * resolution + i] = 1;
	    		}
	    	}
	    }

	    // This loop sets the playable area in main body
	    for (int j = 0; j< 250; j++)
	    {
	    	for (int i = 0; i< 250; i++)
	    	{
	    		aM[j * 250 + i] = walkMap[(j / 4) * resolution + (i / 4)];
	    	}
	    }
     }

    void SmallObjects(int[] aM, int w, int h)
    {
        System.Random rand = new System.Random();
        int eCount = rand.Next(30, 60);

        // Loop until all enemy locations placed
        for (int k = 0; k < eCount; k++)
        {
            bool siteChosen = false;
            // Loop until enemy is successfully positioned
            while (!siteChosen)
            {
                // Pseudo-random coordinates
                int x = rand.Next(0, w);
                int y = rand.Next(0, h);

                // Check that location is within walkable area
                if (aM[y * w + x] == 0)
                {
                    // Accept as successful may require additional checks
                    siteChosen = true;

                    // Place holder goes here
                    //MarkArea(aM, w, h, x, y, 8, 8, -4);
                    aM[y * w + x] = -4;

                    Vector3 ePosition = new Vector3(-125 + x, 1 + 25 * (averageHeight * 0.9f), -125 + y);

                    Transform sMarker = SmallMarker;
                    Instantiate(sMarker, ePosition, Quaternion.identity, SmallObjectLocations.transform);
                    sMarker.GetComponent<MarkerScript>().SetPanel(InfoPanel, 0);
                }
            }
        }
    }

    void LargeObjects()
    {

    }

    void GenerateEnemyLocations(int[] aM, int w, int h)
    {
        System.Random rand = new System.Random();
        int eCount = rand.Next(30, 60);

        // Loop until all enemy locations placed
        for (int k = 0; k < eCount; k++)
        {
            bool siteChosen = false;
            // Loop until enemy is successfully positioned
            while (!siteChosen)
            {
                // Pseudo-random coordinates
                int x = rand.Next(0, w);
                int y = rand.Next(0, h);

                // Check that location is within walkable area
                if (aM[y * w + x] == 0)
                {
                    // Accept as successful may require additional checks
                    siteChosen = true;

                    // Place holder goes here
                    //MarkArea(aM, w, h, x, y, 8, 8, -3);
                    aM[y * w + x] = -3;

                    Vector3 ePosition = new Vector3(-125 + x, 1 + 25 * (averageHeight * 0.9f), -125 + y);

                    Transform eMarker = EnemyMarker;
                    Instantiate(eMarker, ePosition, Quaternion.identity, EnemyLocations.transform);
                    eMarker.GetComponent<MarkerScript>().SetPanel(InfoPanel, 1);
                }
            }
        }
    }

    void GeneratePlayerLocation(int[] aM, int w, int h)
    {
        System.Random rand = new System.Random();
        bool siteChosen = false;
        // Loop until enemy is successfully positioned
        while (!siteChosen)
        {
            // Pseudo-random coordinates
            int x = rand.Next(0, w);
            int y = rand.Next(0, h);

            // Check that location is within walkable area
            if (aM[y * w + x] == 0)
            {
                // Accept as successful may require additional checks
                siteChosen = true;

                // Place holder goes here
                //MarkArea(aM, w, h, x, y, 8, 8, -3);
                aM[y * w + x] = -5;

                Vector3 pPosition = new Vector3(-125 + x, 1 + 25 * (averageHeight * 0.9f), -125 + y);
                playerObject.transform.position = pPosition;
            }
        }        
    }

    void MarkArea(int[] map, int tw, int th, int x, int y, int w, int h, int v)
    {
    	 for (int j = 0; j<h; j++)
    	 {
    		 for (int i = 0; i<w; i++)
    		 {
    			 map[(y - (h / 2) + j) * tw + (x - (w / 2) + i)] = v;
    		 }
    	 }
    }
       
    void MapCombination(float[] dM, int[] aM, int w, int h)
    {
        // Get average height

        int counter = 0;
        float waterHeight = 0;

        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                averageHeight += dM[j * w + i];
                counter++;
            }
        }

        // Divide sum of heights by count
        averageHeight /= (float)counter;

        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                // Combine the maps
                if (aM[j * w + i] == 0)
                {
                    dM[j * w + i] = averageHeight * 0.9f;
                }
            }
        }

        // Calculate water level
        waterHeight = averageHeight * 0.7f;

        // Set terrain vertice position
        MeshFilter mf = terrainObject.GetComponent<MeshFilter>();
        Vector3[] vertices = mf.mesh.vertices;
        Vector2[] uvs = mf.mesh.uv;

        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                vertices[j * w + i].y = 25 * dM[j * w + i];

                // Calculate colour
                float maxHeight = 1.0f;
                float pathHeight = averageHeight * 0.9f;
                float minHeight = waterHeight;
                float lowSplit = minHeight + ((pathHeight - minHeight) / 2);
                float highSplit = pathHeight + ((maxHeight - pathHeight) / 2);

                if (dM[j * w + i] == pathHeight)
                {
                    uvs[j * w + i].x = 0.2f;
                    uvs[j * w + i].y = 0.8f;
                }
                else if (dM[j * w + i] < pathHeight && dM[j * w + i] > lowSplit)
                {
                    uvs[j * w + i].x = 0.2f;
                    uvs[j * w + i].y = 0.2f;
                }
                else if (dM[j * w + i] < lowSplit)
                {
                    uvs[j * w + i].x = 0.5f;
                    uvs[j * w + i].y = 0.2f;
                }
                else if (dM[j * w + i] > pathHeight && dM[j * w + i] < highSplit)
                {
                    uvs[j * w + i].x = 0.5f;
                    uvs[j * w + i].y = 0.8f;
                }
                else
                {
                    uvs[j * w + i].x = 0.8f;
                    uvs[j * w + i].y = 0.8f;
                }
            }
        }

        mf.mesh.vertices = vertices;
        mf.mesh.uv = uvs;
        mf.mesh.RecalculateNormals();

        // Recalculate collision mesh
        terrainObject.GetComponent<MeshCollider>().sharedMesh = mf.mesh;

        // Set water height
        Vector3 waterPosition = new Vector3(0, 1 + 25 * waterHeight, 0);
        waterObject.transform.position = waterPosition;
    }
}
