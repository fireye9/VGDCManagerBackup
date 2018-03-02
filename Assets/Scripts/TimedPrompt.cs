using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimedPrompt : MonoBehaviour {

    public GameObject PopupPrefab;
    public GameObject PopupProduct;
    public GameObject promptPrefab;
    GameObject newPrompt;
    public TextAsset textFilePrompts;
    public TextAsset textFileChoices;
    public TextAsset textFileEffects;
    public Text textObject;
    private string[] prompts;
    private string[][] choices;
    private ChoiceEffect[] effectsArray;
    int rnd;


    GameStateManager gsm;
    struct ChoiceEffect
    {
        public int satisfactionEffect;
        public int productionEffect;
        public ChoiceEffect(int sValue, int pValue)
        {
            this.satisfactionEffect = sValue;
            this.productionEffect = pValue;
        }
    }

    private void Awake()
    {
        gsm = GetComponent(typeof(GameStateManager)) as GameStateManager;
        
        prompts = textFilePrompts.text.Split('\n');
        effectsArray = new ChoiceEffect[prompts.Length * 3];

        choices = new string[prompts.Length][];
        string[] choiceLines = textFileChoices.text.Split('\n');
        for (int i = 0; i < choiceLines.Length; i++)
            choices[i] = choiceLines[i].Split('|');

        string[] effectSplit = textFileEffects.text.Split(';');

        for (int i = 0; i < prompts.Length * 3; i++)
        {
            string[] pairSplit = effectSplit[i].Split(',');
            ChoiceEffect newEffect = new ChoiceEffect( int.Parse(pairSplit[0]), int.Parse(pairSplit[1]) );
            effectsArray[i] = newEffect;
        }
    }
    public void CreateNewPrompt()
    {
        newPrompt = Instantiate(promptPrefab);
        newPrompt.transform.SetParent(GameObject.Find("UICanvas").transform,false);

        
        textObject = newPrompt.transform.Find("PromptText").GetComponent<Text>();

        //Select random prompt
        rnd = Random.Range(0, prompts.Length);

        textObject.text = prompts[rnd];


        //Insert text
        for (int i = 0; i < 3; i++)
        {
            Transform child;
            child = newPrompt.transform.Find("Choices").GetChild(i);
            child.Find("Text").GetComponent<Text>().text = choices[rnd][i];
        }

        Button[] buttons = newPrompt.GetComponentsInChildren<Button>();

        foreach (Button bt in buttons)
        {
            bt.onClick.AddListener(delegate { OnChoiceClick(); });
        }

    }
    public void OnChoiceClick()
    {
        ChoiceStatsChange(EventSystem.current.currentSelectedGameObject.name);
        if (gsm.week != 10)
        {
            gsm.week += 1;
            gsm.day = 1f;
            Time.timeScale = 1;
            Destroy(newPrompt);
        }
        else
            Destroy(newPrompt);
    }
    void ChoiceStatsChange(string choiceName)
    {
        int targetChoice = -1;
        if (choiceName == "Choice1")
            targetChoice = rnd * 3;
        else if (choiceName == "Choice2")
            targetChoice = rnd * 3 + 1;
        else if (choiceName == "Choice3")
            targetChoice = rnd * 3 + 2;

        if (targetChoice >= 0)
        {
            float satEffect = effectsArray[targetChoice].satisfactionEffect;
            float prodEffect = effectsArray[targetChoice].productionEffect;
            gsm.SetSatisfaction(gsm.satisfaction + satEffect);
            gsm.SetProductivity(gsm.productivity + prodEffect);

            string satPop = satEffect.ToString();
            if (satEffect > 0)
                satPop = "+" + satPop;
            string prodPop = prodEffect.ToString();
            if (prodEffect > 0)
                prodPop = "+" + prodPop;

            //Pop up text to show changed values
            GameObject newTextSat = Instantiate(PopupPrefab);
            newTextSat.transform.SetParent(GameObject.Find("UICanvas").transform, false);
            newTextSat.GetComponent<GStatePopupText>().SetText(satPop);

            GameObject newTextProd = Instantiate(PopupProduct);
            newTextProd.GetComponent<GStatePopupText>().SetText(prodPop);
            newTextProd.transform.SetParent(GameObject.Find("UICanvas").transform, false);
        }
    }
    
}
