using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerScript : MonoBehaviour
{
    // User interface element
    public GameObject InfoPanel;

    // Scripts
    private CSVReader csvScript;

    // Important vars
    public string type;
    public int coin;
    public int itemCount;
    public List<int> contents;

	// Use this for initialization
	void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnMouseDown()
    {
        InfoPanel.SetActive(true);

        // Set data in panel
        InfoPanel.transform.GetChild(1).GetComponentInChildren<Text>().text = "Type: " + type + "\n";

        InfoPanel.transform.GetChild(2).GetComponentInChildren<Text>().text = "Position: " + transform.position.x
            + ", " + transform.position.y + ", " + transform.position.z + "\n";

        InfoPanel.transform.GetChild(3).GetComponentInChildren<Text>().text = "Rotation: " + transform.rotation.x
            + ", " + transform.rotation.y + ", " + transform.rotation.z + "\n";

        csvScript = GetComponent<CSVReader>();

        string contentText = "Contents:\n";

        if (type != "Enemy Group")
        {            
            for (int i = 0; i < itemCount; i++)
            {
                contentText += csvScript.LoadData(contents[i], 0, "LootTable") + ", ";
                contentText += csvScript.LoadData(contents[i], 1, "LootTable") + ", ";
                contentText += csvScript.LoadData(contents[i], 2, "LootTable") + "-";
                contentText += csvScript.LoadData(contents[i], 3, "LootTable") + ", ";
                contentText += csvScript.LoadData(contents[i], 4, "LootTable") + ", ";
                contentText += coin.ToString() + "\n";
            }
        }
        else
        {
            for (int i = 0; i < itemCount; i++)
            {
                contentText += csvScript.LoadData(contents[i], 0, "EnemyTable") + "\n";
            }
        }

        InfoPanel.transform.GetChild(4).GetComponentInChildren<Text>().text = contentText;
    }

    public void SetPanel(GameObject p, int t)
    {
        InfoPanel = p;
        contents.Clear();

        switch (t)
        {
            case 0:
                // Small object
                type = "Chest";

                // Amount of coin
                coin = Random.Range(10, 251);

                // Number of items
                itemCount = Random.Range(2, 13);

                for (int i = 0; i < itemCount; i++)
                {
                    // Generate item numbers (for now avoiding duplicates within single cache)
                    bool chosen = true;
                    int tempNumber;

                    do
                    {
                        tempNumber = Random.Range(1, 41);

                        for (int j = 0; j < contents.Count; j++)
                        {
                            if (contents.Count > 0)
                            {
                                if (contents[j] == tempNumber)
                                {
                                    chosen = false;
                                }
                                else
                                {
                                    chosen = true;
                                }
                            }                            
                        }

                    } while (!chosen);

                    contents.Add(tempNumber);
                }

                break;

            case 1:
                // Enemy group
                type = "Enemy Group";

                itemCount = Random.Range(1, 13);

                for (int i = 0; i < itemCount; i++)
                {
                    // Select type of unit
                    int unitType = Random.Range(0, 9);

                    // Not restricted atm
                    contents.Add(unitType);
                }

                break;
        }
    }
}
