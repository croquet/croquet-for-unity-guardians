using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthCoinController : MonoBehaviour
{
    public TMP_Text healthTextFront;
    public TMP_Text healthTextBack;
    
    void Start()
    {
        Croquet.Subscribe("stats", "health", SetHealth);
    }
    
    void SetHealth(float health)
    {
        healthTextFront.text = $"{health}";
        healthTextBack.text = $"{health}";
    }
}
