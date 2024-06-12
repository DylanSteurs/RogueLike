using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class FloorInfo : MonoBehaviour
{
    public Label enemiesLeftText;
    public Label floorText;

    // Update the enemies left text
    public void UpdateEnemiesLeft(int enemiesLeft)
    {
        enemiesLeftText.text = $"{enemiesLeft} enemies left";
    }

    // Update the floor text
    public void UpdateFloor(int floor)
    {
        floorText.text = $"Floor {floor}";
    }
}
