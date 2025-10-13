using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSpawner : MonoBehaviour
{
    [Header("Spawning Setup")]
    public GameObject bookPrefab; // Drag your book prefab here
    public Transform centerPoint; // The point the books will circle around
    public int maxBooks = 25;
    public float spawnInterval = 1.5f; // Time between each book spawn

    [Header("Movement Progression")]
    [Tooltip("The speed of the first book spawned.")]
    public float minRotationSpeed = 1f;
    [Tooltip("The speed when the maximum number of books is reached.")]
    public float maxRotationSpeed = 4f;

    [Header("Wildness Progression")]
    [Tooltip("How much the flight path radius can randomly vary.")]
    public float radiusWildness = 2f;
    [Tooltip("How much the flight path height can randomly vary.")]
    public float elevationWildness = 1.5f;

    // --- Private Variables ---
    private List<GameObject> spawnedBooks = new List<GameObject>();
    private float spawnTimer;

    void Update()
    {
        // Only spawn if we haven't reached the max count
        if (spawnedBooks.Count >= maxBooks) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnBook();
            spawnTimer = spawnInterval; // Reset timer
        }
    }

    void SpawnBook()
    {
        // --- Calculate Progression ---
        // This value (from 0 to 1) represents how "full" the swarm is.
        float progress = (float)spawnedBooks.Count / maxBooks;

        // --- Instantiate and Position ---
        // Create the book at the center point; the controller will move it.
        GameObject newBook = Instantiate(bookPrefab, centerPoint.position, Quaternion.identity);

        // --- Determine Properties for this new book ---
        // Speed increases linearly from min to max as more books are spawned.
        float currentSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, progress);

        // Add "wildness" by randomizing flight parameters.
        // As progress increases, the potential for wildness also increases.
        float radius = 3f + Random.Range(-radiusWildness * progress, radiusWildness * progress);
        float elevation = 2f + Random.Range(-elevationWildness * progress, elevationWildness * progress);
        float bobSpeed = 2f + Random.Range(-1f, 1f);
        float bobHeight = 0.2f + Random.Range(-0.1f, 0.1f);

        // --- Initialize the Book ---
        BookController controller = newBook.GetComponent<BookController>();
        if (controller != null)
        {
            // The typo was here! Corrected to 'bobHeight'
            controller.Initialize(centerPoint, radius, currentSpeed, elevation, bobSpeed, bobHeight);
        }

        // Add to our list to keep track
        spawnedBooks.Add(newBook);
    }
}