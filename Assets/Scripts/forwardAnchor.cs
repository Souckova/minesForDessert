using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class XRForwardAligner : MonoBehaviour
{
    public Transform forwardAnchor;

    IEnumerator Start()
    {
        InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);

        bool userPresent = false;

        while (!userPresent)
        {
            if (headDevice.isValid)
                headDevice.TryGetFeatureValue(CommonUsages.userPresence, out userPresent);

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        AlignToForwardAnchor();
    }

    void AlignToForwardAnchor()
    {
        Transform cam = Camera.main.transform;

        Vector3 headsetForward = cam.forward;
        headsetForward.y = 0;
        headsetForward.Normalize();

        Vector3 targetForward = forwardAnchor.forward;
        targetForward.y = 0;
        targetForward.Normalize();

        float angle = Vector3.SignedAngle(headsetForward, targetForward, Vector3.up);

        transform.Rotate(0, angle, 0);
    }
}