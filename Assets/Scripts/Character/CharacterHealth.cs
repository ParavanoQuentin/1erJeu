using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarImage; // Drag the child Image here
    [Range(0, 1)] public float health = 1f; // Variable pour simuler les changements

    void Update()
    {
        // Mettre à jour la barre de vie
        healthBarImage.fillAmount = health;
    }
}