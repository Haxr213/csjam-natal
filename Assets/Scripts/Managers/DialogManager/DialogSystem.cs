using UnityEngine;

public enum STATE {
    DISABLED,
    WAITING,
    TYPING
}
public class DialogSystem : MonoBehaviour
{
    public DialogData dialogData;

    int currentText = 0;
    bool finished = false;

    TypeTextAnimation typeText;
    STATE state;
    [SerializeField] private float waitTime = 5f; // tempo em segundos antes de avançar
    private float waitTimer = 0f;

    void Awake()
    {
        //FindAnyObjectOfType
        typeText = FindAnyObjectByType<TypeTextAnimation>();

        typeText.TypeFinished = OnTypeFinished;
    }
    void Start()
    {
        state = STATE.DISABLED;
    }

    void Update()
    {
        if (state == STATE.DISABLED) return;

        switch (state)
        {
            case STATE.WAITING:
                Waiting();
                break;
            case STATE.TYPING:
                Typing();
                break;
        }
    }

    public void Next()
    {
        typeText.fullText = dialogData.talkScript[currentText++].text;

        if(currentText == dialogData.talkScript.Count) finished = true;

        typeText.StartTyping();
        state = STATE.TYPING;
    }

    void OnTypeFinished()
    {
        waitTimer = 0f;
        state = STATE.WAITING;
    }

    void Waiting()
    {
        if(!finished)
        {
            // aguarda por `waitTime` segundos antes de avançar
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                waitTimer = 0f;
                Next();
            }
        } else
        {
            state = STATE.DISABLED;
            currentText = 0;
            finished = false;
        }
    }

    void Typing()
    {

    }
}
