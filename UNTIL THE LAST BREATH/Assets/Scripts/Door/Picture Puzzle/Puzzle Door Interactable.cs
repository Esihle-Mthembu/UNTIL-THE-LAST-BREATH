using UnityEngine;

public class PuzzleDoorInteractable : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject puzzleCanvas;
    [SerializeField] private PuzzleGridController puzzleGrid;
    //[SerializeField] private DoorController door;

    [Header("Player Control Lock (assign your FP scripts)")]
    [SerializeField] private MonoBehaviour[] scriptsToDisableWhileSolving;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private LayerMask interactMask = ~0;

    private Camera playerCamera;
    private bool inPuzzleMode = false;
    private bool alreadySolved = false;

    private void Start()
    {
        playerCamera = Camera.main;
        if (puzzleCanvas != null)
            puzzleCanvas.SetActive(false);

        if (puzzleGrid != null)
            puzzleGrid.OnSolved += HandleSolved;
    }

    private void Update()
    {
        if (alreadySolved) return;

        if (!inPuzzleMode)
        {
            if (IsPlayerLookingAtDoor() && Input.GetKeyDown(interactKey))
            {
                EnterPuzzleMode();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitPuzzleMode();
            }
        }
    }

    private bool IsPlayerLookingAtDoor()
    {
        if (playerCamera == null) return false;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactMask))
        {
            return hit.collider.gameObject == gameObject
                   || hit.collider.transform.IsChildOf(transform);
        }
        return false;
    }

    private void EnterPuzzleMode()
    {
        inPuzzleMode = true;

        puzzleCanvas.SetActive(true);
        puzzleGrid.enabled = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetPlayerControlEnabled(false);
    }

    private void ExitPuzzleMode()
    {
        inPuzzleMode = false;

        puzzleCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetPlayerControlEnabled(true);
    }

    private void SetPlayerControlEnabled(bool enabled)
    {
        foreach (var script in scriptsToDisableWhileSolving)
        {
            if (script != null)
                script.enabled = enabled;
        }
    }

    private void HandleSolved()
    {
        alreadySolved = true;
        Invoke(nameof(FinishAndOpenDoor), 1f); // A delay so the player can see the completed picture before it closes
    }

    private void FinishAndOpenDoor()
    {
        ExitPuzzleMode();
        //door.Open();
    }
}
