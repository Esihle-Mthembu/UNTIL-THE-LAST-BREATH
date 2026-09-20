using UnityEngine;

public class HauntedDoor : MonoBehaviour
{
    public Animator animator;
    public float minInterval = 5f;
    public float maxInterval = 15f;

    private bool isOpen = false;

    void Start()
    {
        ScheduleNextAction();
    }

    void ScheduleNextAction()
    {
        float delay = Random.Range(minInterval, maxInterval);
        Invoke(nameof(ToggleDoor), delay);
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;
        animator.SetBool("IsOpen", isOpen);
        ScheduleNextAction();
    }
}