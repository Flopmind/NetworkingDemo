using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    protected Slider healthSlider;

    [SerializeField]
    protected PlayerController player;

    void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        if (player)
        {
            healthSlider.value = player.Health / player.startHealth;
        }
        else
        {
            healthSlider.value = 1;
        }
    }
}
