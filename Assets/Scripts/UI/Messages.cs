using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Messages : MonoBehaviour
{
    private Label[] labels = new Label[5];
    private VisualElement root;

    void Start()
    {
        // Fetch the root VisualElement from the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Initialize the labels array with the corresponding labels from the UI
        for (int i = 0; i < labels.Length; i++)
        {
            labels[i] = root.Q<Label>($"Label{i + 1}");
        }

        // Clear all labels initially
        Clear();

        // Add a welcome message
        AddMessage("Welcome to the dungeon, Adventurer!", Color.cyan);
    }

    public void Clear()
    {
        // Set the text of all labels to an empty string
        foreach (var label in labels)
        {
            label.text = string.Empty;
        }
    }

    public void MoveUp()
    {
        // Move the text and color from each label to the next one
        for (int i = labels.Length - 1; i > 0; i--)
        {
            labels[i].text = labels[i - 1].text;
            labels[i].style.color = labels[i - 1].style.color;
        }
        // Clear the first label
        labels[0].text = string.Empty;
    }

    public void AddMessage(string content, Color color)
    {
        // Move up the messages in the labels
        MoveUp();

        // Set the first label with the new message and color
        labels[0].text = content;
        labels[0].style.color = new StyleColor(color);
    }
}