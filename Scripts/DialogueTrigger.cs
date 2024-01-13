using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogueData; //Scriptable
    private Transform dialogueT;
    private GameObject canvasGO;
    private Text dialogueName;
    private Text dialogueText;
    public SpriteRenderer siblingSprite;
    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        canvasGO = GameObject.Find("Canvas");
        playerGO = GameObject.Find("Player");
        siblingSprite = transform.parent.GetChild(0).GetComponent<SpriteRenderer>();
    }

    int sentences_arr = -1;
    void TriggerDialogue()
    {
        dialogueT = canvasGO.transform.Find("DialogueBox");
        dialogueT.gameObject.SetActive(true);
        dialogueText = dialogueT.transform.Find("DialogueText").GetComponent<Text>();
        dialogueName = dialogueT.transform.Find("DialogueName").GetComponent<Text>();
        if (dialogueData && dialogueT)
        {
            // Assign dialogue content to the UI text element
            dialogueName.text = dialogueData.name;
            dialogueText.text = dialogueData.sentences[sentences_arr];
        }

        CinemachineEffect.Instance.ZoomIn();
    }

    public bool isTriggered = false;
    void CloseDialogue()
    {
        if (dialogueT)
        {
            sentences_arr = -1;
            dialogueT.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") )
        {
            isTriggered = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
        CloseDialogue();
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isTriggered)
        {
            sentences_arr++;
            if (sentences_arr == dialogueData.sentences.Length)
                { CloseDialogue(); return; }
            if (sentences_arr > dialogueData.sentences.Length)
                { sentences_arr = 0; }
            //print("Displaying : " + sentences_arr);
            TriggerDialogue();
        }

        if(dialogueT && dialogueT.gameObject.activeSelf)
        {
            FacePlayer();
        }

        if(gameManager.GetGameplayFrozen())
        {
            CloseDialogue();
        }
    }

    GameObject playerGO;
    SpriteRenderer playerSpriteRenderer;
    void FacePlayer()
    {
        playerSpriteRenderer = playerGO.transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(playerGO.transform.position.x > transform.position.x)
        {
        siblingSprite.flipX = true;
        playerSpriteRenderer.flipX = false;
        }
        else
        {
        siblingSprite.flipX = false;
        playerSpriteRenderer.flipX = true;
        }
    }


}
