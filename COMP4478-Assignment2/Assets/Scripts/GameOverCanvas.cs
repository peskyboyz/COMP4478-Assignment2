using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCanvas : MonoBehaviour
{
    public TextMeshProUGUI messageText;

    private void Start()
    {
        // Hide the canvas on start
        gameObject.SetActive(false);
    }

    public void ShowGameOverScreen(int pairsSelected, int totalPairs)
    {
        // Show the canvas
        gameObject.SetActive(true);

        // Check if the user has selected all the pairs
        if (pairsSelected == totalPairs)
        {
            // Set the win message
            messageText.text = "Congratulations! You have completed the game.";
        }
        else
        {
            // Set the game over message
            messageText.text = "Game over! You selected " + pairsSelected + " pairs.";
        }
    }



    public void RestartGame()
    {
        // Reload the current scene to restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

