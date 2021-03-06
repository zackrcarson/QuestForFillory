using UnityEngine.UI;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    // Config Parameters
    [Header("Parameters")]
    [SerializeField] string playerName = "Quentin";
    [SerializeField] string mysteryName = "???";
    [SerializeField] Color NPCNameColor = new Color(0f, 0f, 0f, 0f);
    [SerializeField] Color playerNameColor = new Color(0f, 0f, 0f, 0f);

    [Header("Configuration")]
    [SerializeField] string dialogueNameIndicator = "n-";
    [SerializeField] string dialogueNPCIndicator = "NPC";
    [SerializeField] string dialoguePlayerIndicator = "PLAYER";
    [SerializeField] string dialogueSignIndicator = "SIGN";
    [SerializeField] public GameObject dialogueBox = null;
    [SerializeField] public GameObject nameBox = null;
    [SerializeField] Text dialogueText = null;
    [SerializeField] Text nameText = null;

    // Cached References
    PlayerController player = null;

    // State Variables
    bool justStarted = false;
    int currentLine = 0;

    string NPCName = null;
    string[] dialogueLines = null;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDialogue();
    }

    private void UpdateDialogue()
    {
        if (dialogueBox.activeInHierarchy && dialogueLines.Length != 0)
        {
            if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Jump") || Input.GetButtonUp("Submit"))
            {
                if (!justStarted)
                {
                    if (currentLine < dialogueLines.Length - 1)
                    {
                        currentLine++;

                        CheckIfName();
                        dialogueText.text = dialogueLines[currentLine];
                    }
                    else
                    {
                        currentLine = 0;

                        dialogueBox.SetActive(false);
                        player.CanMove(true);
                    }
                }
                else
                {
                    justStarted = false;
                }
            }
        }
    }

    public void ShowNewDialogue(string[] newLines, string name)
    {
        dialogueLines = newLines;

        if (name == "")
        {
            NPCName = mysteryName;
        }
        else
        {
            NPCName = name;
        }

        currentLine = 0;

        CheckIfName();
        dialogueText.text = dialogueLines[currentLine];

        if (name == dialogueSignIndicator)
        {
            nameBox.SetActive(false);
        }
        else
        {
            nameBox.SetActive(true);
        }

        dialogueBox.SetActive(true);

        justStarted = true;

        if (!player) { player = FindObjectOfType<PlayerController>(); }
        player.CanMove(false);
    }

    void CheckIfName()
    {
        string tempLine = dialogueLines[currentLine];

        if (currentLine == 0 && !tempLine.StartsWith(dialogueNameIndicator))
        {
            nameText.color = NPCNameColor;
            nameText.text = mysteryName;
        }

        if (tempLine.StartsWith(dialogueNameIndicator))
        {
            tempLine = tempLine.Replace(dialogueNameIndicator, "");

            if (tempLine == dialogueNPCIndicator)
            {
                nameText.color = NPCNameColor;
                nameText.text = NPCName;
            }
            else if (tempLine == dialoguePlayerIndicator)
            {
                nameText.color = playerNameColor;
                nameText.text = playerName;
            }
            
            currentLine++;
        }
    }
}
