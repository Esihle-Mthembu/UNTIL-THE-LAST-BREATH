using UnityEngine;


[RequireComponent(typeof(Collider))]
public class DoorOpen : MonoBehaviour
{
    [Header("Rotation Settings")]
    [Tooltip("How many degrees the door should open.")]
    public float openAngle = 90f;

    [Tooltip("How fast the door rotates (degrees per second).")]
    public float openSpeed = 120f;

    [Header("Behaviour")]
    [Tooltip("If true, the door can be closed again by touching it while open.")]
    public bool toggleOnTouch = true;

    private Quaternion closedRotation;
    private Quaternion targetRotation;
    private bool isOpen = false;
    private bool isMoving = false;

    private void Start()
    {
        closedRotation = transform.rotation;
        targetRotation = closedRotation;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                openSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.05f)
            {
                transform.rotation = targetRotation;
                isMoving = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!isOpen)
        {
            OpenAwayFrom(other.transform);
        }
        else if (toggleOnTouch)
        {
            Close();
        }
    }

    private void OpenAwayFrom(Transform playerTransform)
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        Vector3 localDirection = transform.InverseTransformDirection(directionToPlayer);

        float direction = (localDirection.z > 0f) ? -1f : 1f;

        targetRotation = closedRotation * Quaternion.Euler(0f, openAngle * direction, 0f);
        isOpen = true;
        isMoving = true;
    }

    private void Close()
    {
        targetRotation = closedRotation;
        isOpen = false;
        isMoving = true;
    }
}