using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AssignPlayerCount : MonoBehaviour
{
    public TMP_Text playerCountTextToUpdate;

    void Awake()
    {
        Croquet.Subscribe("croquet", "viewCount", UpdatePlayerCount);
    }

    void UpdatePlayerCount(float playerCount)
    {
        // Debug.Log("Updating Player Count!");
        playerCountTextToUpdate.text = Mathf.FloorToInt(playerCount).ToString();
    }
}
