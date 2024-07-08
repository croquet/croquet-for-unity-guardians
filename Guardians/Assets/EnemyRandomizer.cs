using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomizer : MonoBehaviour
{
    public List<GameObject> enemies;
    private CroquetEntityComponent croquetEntityComponent;

    void Start()
    {
        croquetEntityComponent = GetComponent<CroquetEntityComponent>();

        // Extract integer from croquetActorId
        string actorId = croquetEntityComponent.croquetActorId;
        int handle = int.Parse(actorId.Substring(1));

        // Determine the index using the last digit
        int lastDigit = handle % 10;
        int chosenIndex;

        if (lastDigit % 2 == 0)
        {
            chosenIndex = 0; // 50% chance for the 0 entry
        }
        else
        {
            // Map last digit to index
            switch (lastDigit)
            {
                case 1:
                case 7:
                    chosenIndex = 1;
                    break;
                case 3:
                case 9:
                    chosenIndex = 2;
                    break;
                case 5:
                    chosenIndex = 3;
                    break;
                default:
                    chosenIndex = 0; // Fallback, should not happen
                    break;
            }
        }

        // Activate only the chosen enemy
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetActive(i == chosenIndex);
        }
        // Debug.Log("Chosen enemy: " + enemies[chosenIndex].name + " (handle: " + handle + ")" + " (index: " + chosenIndex + ")" + croquetEntityComponent.croquetActorId);
    }

    void Update()
    {
        // Add any necessary update logic here
    }
}
