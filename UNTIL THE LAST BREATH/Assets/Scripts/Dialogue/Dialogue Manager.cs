using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public DialogueData introDialogue;

    public GameObject dialoguePanel;
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;

    public InputAction advanceDialogue;

    [Header("Typewriter Settings")]
    public float typingSpeed = 0.05f;

    private int currentLine = 0;
    private bool dialogueActive = false;
    private bool isTyping = false;

    private Coroutine typingCoroutine;

    [Header("Scene Transition")]
    public string nextSceneName = "Gameplay Scene";

    private void OnEnable()
    {
        advanceDialogue.Enable();
        advanceDialogue.performed += OnNextDialogue;
    }

    private void OnDisable()
    {
        advanceDialogue.performed -= OnNextDialogue;
        advanceDialogue.Disable();
    }

    private void Start()
    {
        StartDialogue();
    }

    public void StartDialogue()
    {
        currentLine = 0;
        dialogueActive = true;

        dialoguePanel.SetActive(true);

        ShowLine();
    }

    private void ShowLine()
    {
        DialogueData.DialogueLine line =
            introDialogue.dialogueLines[currentLine];

        speakerNameText.text = line.speakerName;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeLine(line.dialogueText));
    }

    private IEnumerator TypeLine(string text)
    {
        isTyping = true;

        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void OnNextDialogue(InputAction.CallbackContext context)
    {
        if (!dialogueActive)
            return;

        Debug.Log("Dialogue input detected!");

        if (isTyping)
        {
            FinishCurrentLine();
        }
        else
        {
            NextLine();
        }
    }

    private void FinishCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogueText.text =
            introDialogue.dialogueLines[currentLine].dialogueText;

        isTyping = false;
    }

    private void NextLine()
    {
        currentLine++;

        if (currentLine < introDialogue.dialogueLines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueActive = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialoguePanel.SetActive(false);

        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeAndLoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("ScreenFader not found in scene — loading directly.");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}