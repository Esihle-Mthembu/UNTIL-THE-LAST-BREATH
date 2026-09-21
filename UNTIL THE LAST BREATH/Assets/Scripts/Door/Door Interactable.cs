using UnityEngine;

public class DoorInteractable : MonoBehaviour
{
    [Header("Lock Settings")]
    public bool requiresPin = true;
    public bool isUnlocked = false;

    [Header("Door Movement")]
    public Transform doorTransform;
    public float openAngle = 90f;
    public float openSpeed = 2f;

    public PinPadUI linkedPinPad;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        if (doorTransform == null)
        {
            doorTransform = transform;
        }

        closedRotation = doorTransform.localRotation;
        openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
    }

    public void Interact()
    {
        if (requiresPin && !isUnlocked)
        {
            if (linkedPinPad != null)
            {
                linkedPinPad.Open(this);
            }
            
            return;
        }

        ToggleDoor();
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpen ? openRotation : closedRotation));
    }

    private System.Collections.IEnumerator RotateDoor(Quaternion target)
    {
        while (Quaternion.Angle(doorTransform.localRotation, target) > 0.5f)
        {
            doorTransform.localRotation = Quaternion.Slerp(
                doorTransform.localRotation, target, Time.deltaTime * openSpeed);
            yield return null;
        }
        doorTransform.localRotation = target;
    }

    public void Unlock()
    {
        isUnlocked = true;

        ToggleDoor();
    }
}
