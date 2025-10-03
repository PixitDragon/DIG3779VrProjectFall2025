using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
[RequireComponent(typeof(AudioSource))]
public class Glowstick : MonoBehaviour
{
    [Header("Glow Settings")]
    [Tooltip("The color of the light and material emission.")]
    public Color glowColor = Color.green;

    [Tooltip("The brightness of the point light.")]
    [SerializeField] private float lightIntensity = 2f;

    [Tooltip("The range of the point light.")]
    [SerializeField] private float lightRange = 5f;

    [Header("Sound")]
    [Tooltip("The 'crack' sound played upon activation.")]
    [SerializeField] private AudioClip crackSound;

    // --- Component References ---
    private Light pointLight; // CORRECTED LINE: The class is 'Light', not 'PointLight'.
    private Renderer objectRenderer;
    private AudioSource audioSource;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    // --- State ---
    private bool isActivated = false;

    void Awake()
    {
        // Get references to all necessary components
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        objectRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        // Find or create the Point Light component
        pointLight = GetComponentInChildren<Light>(); // CORRECTED LINE
        if (pointLight == null)
        {
            GameObject lightGameObject = new GameObject("Glowstick Light");
            lightGameObject.transform.SetParent(this.transform);
            lightGameObject.transform.localPosition = Vector3.zero;
            pointLight = lightGameObject.AddComponent<Light>(); // CORRECTED LINE
            pointLight.type = LightType.Point; // This line ensures it's a Point Light
        }

        // --- Initial Setup ---
        pointLight.enabled = false;
        audioSource.playOnAwake = false;
        objectRenderer.material = new Material(objectRenderer.material);
    }

    private void OnEnable()
    {
        grabInteractable.activated.AddListener(Activate);
    }

    private void OnDisable()
    {
        grabInteractable.activated.RemoveListener(Activate);
    }

    public void Activate(ActivateEventArgs arg)
    {
        if (isActivated) return;
        isActivated = true;

        pointLight.enabled = true;
        pointLight.color = glowColor;
        pointLight.intensity = lightIntensity;
        pointLight.range = lightRange;

        objectRenderer.material.EnableKeyword("_EMISSION");
        objectRenderer.material.SetColor("_EmissionColor", glowColor * 2.0f);

        if (crackSound != null)
        {
            audioSource.PlayOneShot(crackSound);
        }
    }
}