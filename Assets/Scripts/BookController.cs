using UnityEngine;

public class BookController : MonoBehaviour
{
    [Header("Material Settings")]
    [Tooltip("The slot for the cover material in the Mesh Renderer. 0 is the first, 1 is the second, etc.")]
    public int coverMaterialIndex = 1;

    // Private variables set by the spawner
    private Transform centerPoint;
    private float flightRadius;
    private float rotationSpeed;
    private float elevation;
    private float bobSpeed;
    private float bobHeight;
    private float angle = 0f;

    public void Initialize(Transform center, float radius, float speed, float elev, float bSpeed, float bHeight)
    {
        centerPoint = center;
        flightRadius = radius;
        rotationSpeed = speed;
        elevation = elev;
        bobSpeed = bSpeed;
        bobHeight = bHeight;
        angle = Random.Range(0f, 360f);

        SetRandomCoverColor();
    }

    void SetRandomCoverColor()
    {
        Renderer bookRenderer = GetComponent<Renderer>();

        if (bookRenderer == null || coverMaterialIndex >= bookRenderer.materials.Length)
        {
            Debug.LogError("Renderer or Material Index is invalid on the book!", this.gameObject);
            return;
        }

        Material[] materials = bookRenderer.materials;
        // This now generates fully saturated, vibrant colors.
        materials[coverMaterialIndex].color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
        materials[coverMaterialIndex].color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
    }

    void Update()
    {
        if (centerPoint == null) return;

        angle += rotationSpeed * Time.deltaTime;
        float x = Mathf.Cos(angle) * flightRadius;
        float z = Mathf.Sin(angle) * flightRadius;
        float yBob = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(centerPoint.position.x + x, centerPoint.position.y + elevation + yBob, centerPoint.position.z + z);
        transform.LookAt(centerPoint);
    }
}