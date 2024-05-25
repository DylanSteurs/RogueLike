using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    private VisualElement root;
    private VisualElement healthBar;
    private Label healthText;

    // Start is called before the first frame update
    void Start()
    {
        // Fetch the root VisualElement from the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Fetch the health bar and text elements
        healthBar = root.Q<VisualElement>("HealthBar");
        healthText = root.Q<Label>("HealthText");

        // Check if elements are found
        if (healthBar == null)
        {
            Debug.LogError("HealthBar element is not found in the UXML!");
        }
        if (healthText == null)
        {
            Debug.LogError("HealthText element is not found in the UXML!");
        }

        // Initialize the health bar and label with default values
        SetValues(30, 30); // Example initialization
    }

    // Method to set the health values and update the UI
    public void SetValues(int currentHitPoints, int maxHitPoints)
    {
        if (healthBar == null || healthText == null)
        {
            Debug.LogError("HealthBar or HealthText element is not initialized!");
            return;
        }

        // Calculate the percentage of hit points remaining
        float percent = (float)currentHitPoints / maxHitPoints * 100;

        // Update the width of the health bar
        healthBar.style.width = new Length(percent, LengthUnit.Percent);

        // Update the text of the health label
        healthText.text = $"{currentHitPoints}/{maxHitPoints} HP";
    }
}