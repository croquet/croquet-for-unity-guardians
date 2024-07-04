using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomizer : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<int> enemyProbabilities;

    void Start()
    {
        int randomValue = Random.Range(0, 10000);

        int cumulativeProbability = 0;
        int chosenIndex = 0;

        for (int i = 0; i < enemyProbabilities.Count; i++)
        {
            cumulativeProbability += enemyProbabilities[i];
            if (randomValue < cumulativeProbability)
            {
                chosenIndex = i;
                break;
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (i != chosenIndex)
            {
                enemies[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        
    }
}
