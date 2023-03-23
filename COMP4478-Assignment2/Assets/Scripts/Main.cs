using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject cardPrefab;           // This should be assigned in the Inspector
    public Material[] frontCardMaterials;   // This should be assigned in the Inspector
    public Material backCardMaterial;
    public Material[] tempCardMaterial;
    public int pairs = 8;                   // The number of card pairs you want to create
    public float flipDuration;
    public float flipDelay;
    private Card firstCardSelected;
    private int pairsSelected;
    private bool isGameOver = false;
    public Canvas gameOverCanvasRef;

    // Start is called before the first frame update
    void Start()
    {
        int[] cardPairs = new int[] {0,0,1,1,2,2,3,3,4,4,5,5,6,6,7,7};

        // Shuffle the card pairs
        ShuffleArray(cardPairs);

        // Create the cards and assign them a pair from the shuffled array

        float startX = -1.8f;
        float startY = 0.51f;
        float startZ = 1f;
        float offsetX = 1.2f;
        float offsetZ = -1.2f;
        int index = 0;
        Quaternion cardRotation = Quaternion.Euler(90f, 0f, 0f);

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                // Create a new card at the current position and assign it the next card pair
                GameObject newCard = Instantiate(cardPrefab, new Vector3(startX + offsetX * col, startY, startZ + offsetZ * row), cardRotation);

                // Get the Mesh Renderer component of the card
                MeshRenderer meshRenderer = newCard.GetComponent<MeshRenderer>();
                Material[] newMaterial = new Material[] { backCardMaterial, frontCardMaterials[cardPairs[index]] };
                tempCardMaterial = meshRenderer.materials;
                for (int i = 0; i < meshRenderer.materials.Length; i++)
                {
                    tempCardMaterial[i] = newMaterial[i];
                }

                // Assign the material to the card based on its index
                meshRenderer.materials = tempCardMaterial;
                // Get the Card component of the card
                Card cardScript = newCard.GetComponent<Card>();
                // Assign the card value to the Card component
                cardScript.cardValue = cardPairs[index];
                // Increment the index for the next card
                index++;
            }
        }
        Invoke("FlipCards", flipDelay);

    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            // The game is over, do nothing
            
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Card"))
                {
                    Card currentCard = hit.collider.gameObject.GetComponent<Card>();

                    if (!currentCard.isFlipped)
                    {
                        // The card is already flipped, do nothing
                        return;
                    }

                    if (firstCardSelected == null)
                    {
                        // This is the first card that the user has selected
                        firstCardSelected = currentCard;
                    }
                    else
                    {
                        // This is the second card that the user has selected
                        if (firstCardSelected.cardValue == currentCard.cardValue)
                        {
                            // The user has selected a matching pair
                            pairsSelected++;

                            // Reset the firstCardSelected variable
                            firstCardSelected = null;

                            // Check if the user has selected all the pairs
                            if (pairsSelected == pairs)
                            {
                                isGameOver = true;
                                // Disable the Collider component of all the cards
                                Card[] cards = FindObjectsOfType<Card>();
                                foreach (Card card in cards)
                                {
                                    card.isFlipped = false;
                                }
                                // Show the game over/restart screen
                                gameOverCanvasRef.gameObject.SetActive(true);
                                GameOverCanvas gameOverCanvas = FindObjectOfType<GameOverCanvas>();
                                gameOverCanvas.ShowGameOverScreen(pairsSelected, pairs);
                            }
                        }
                        else
                        {
                            // The user has selected a non-matching pair
                            StartCoroutine(FlipCard(currentCard.gameObject, flipDuration));

                            // Reset the firstCardSelected variable
                            firstCardSelected = null;
                            
                            // Display a message and end the game
                            isGameOver = true;
                            // Disable the Collider component of all the cards
                            Card[] cards = FindObjectsOfType<Card>();
                            foreach (Card card in cards)
                            {
                                card.isFlipped = false;
                            }
                            // Show the game over/restart screen
                            gameOverCanvasRef.gameObject.SetActive(true);
                            GameOverCanvas gameOverCanvas = FindObjectOfType<GameOverCanvas>();
                            gameOverCanvas.ShowGameOverScreen(pairsSelected, pairs);
                        }
                    }


                    // Flip the current card regardless of whether it's the first or second card selected
                    StartCoroutine(FlipCard(currentCard.gameObject, flipDuration));
                }
            }
        }
    }


    void ShuffleArray<T>(T[] array)
    {
        // Fisher-Yates-Durstenfeld shuffle algorithm
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    void FlipCards()
    {
        // Get all card game objects in the scene
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        
        // Loop through each card and flip it over
        foreach (GameObject card in cards)
        {
            // Lift the card and flip it over
            
            GameObject cardToFlip = card;   // reference to the card object to flip
            StartCoroutine(FlipCard(cardToFlip, flipDuration));
        }
    }
    IEnumerator FlipCard(GameObject card, float duration)
    {
        Rigidbody rb = card.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;

        Vector3 startPosition = card.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0f, 0.35f, 0f);

        Quaternion endRotation;

        float time = 0.0f;
        Quaternion startRotation = card.transform.rotation;
        if (startRotation.eulerAngles.x == 90) { 
            endRotation = Quaternion.Euler(-90.0f, -90.0f, -90.0f);
            // Set the isFlipped flag to true
            card.GetComponent<Card>().isFlipped = true;
        } else
        {
            endRotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);
            // Set the isFlipped flag to true
            card.GetComponent<Card>().isFlipped = false;
        }

        while (time < duration)
        {
            float t = time / duration;
            card.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0.0f;

        while (time < duration)
        {
            float t = time / duration;
            card.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            time += Time.deltaTime;
            yield return null;
        }

        time = 0.0f;

        while (time < duration)
        {
            float t = time / duration;
            card.transform.position = Vector3.Lerp(endPosition, startPosition, t);
            time += Time.deltaTime;
            yield return null;
        }
        card.transform.position = startPosition;
        card.transform.rotation = endRotation;

        rb.useGravity = true;
        rb.isKinematic = false;
    }


}
