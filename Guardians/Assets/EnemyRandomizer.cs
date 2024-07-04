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
        int handle = croquetEntityComponent.croquetHandle;

        // Determine the index using a biased approach
        int chosenIndex;
        if (handle % 2 == 0)
        {
            chosenIndex = 0; // At least 50% chance for the 0 entry
        }
        else
        {
            chosenIndex = (handle / 2) % (enemies.Count - 1) + 1;
        }

        // Activate only the chosen enemy
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetActive(i == chosenIndex);
        }
    }

    void Update()
    {
        // Add any necessary update logic here
    }
}
