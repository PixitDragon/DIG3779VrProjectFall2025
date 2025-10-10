using UnityEngine;

/// <summary>
/// This script rotates the GameObject it is attached to around the Y-axis (horizontally).
/// </summary>
public class HorizontalRotate : MonoBehaviour
{
    // You can change this value in the Unity Inspector to control the speed of the rotation.
    [Tooltip("The speed at which the object will rotate horizontally.")]
    public float rotationSpeed = 50f;

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        // Rotate the transform of the GameObject this script is attached to.
        // We use Vector3.up to specify rotation around the Y-axis (which is the vertical axis, resulting in horizontal rotation).
        // We multiply by Time.deltaTime to make the rotation smooth and independent of the frame rate.
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
