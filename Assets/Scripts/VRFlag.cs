using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Remove this if not using XR Interaction Toolkit

public class VRFlag : MonoBehaviour
{
    private Rigidbody rb;
    private Transform currentStickingZone;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    // Detect when the flag enters the mine's trigger zone
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MineZone"))
        {
            currentStickingZone = other.transform;
        }
    }

    // Detect when the flag leaves the mine's trigger zone
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MineZone") && currentStickingZone == other.transform)
        {
            currentStickingZone = null;
        }
    }

    // Call this method when the VR player GRABS the flag
    public void OnFlagGrabbed()
    {
        // Unparent it in case it was stuck to a mine
        transform.SetParent(null); 
        
        // Re-enable physics so it behaves normally in the hand
        rb.isKinematic = false;
        rb.useGravity = true;
    }

    // Call this method when the VR player RELEASES the flag
    public void OnFlagReleased()
    {
        if (currentStickingZone != null)
        {
            // The player dropped it inside a MineZone!
            
            // 1. Freeze the physics
            rb.isKinematic = true;
            rb.useGravity = false;

            // 2. Parent it to the mine so if the mine moves, the flag moves
            transform.SetParent(currentStickingZone);

            // 3. Optional: Snap it to the center of the mine and make it stand upright
            // transform.position = currentStickingZone.position;
            // transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0); 
        }
        else
        {
            // The player dropped it outside a zone. Let it fall.
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}