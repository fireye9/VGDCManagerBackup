using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptPanel : MonoBehaviour {
    public TextAsset textFilePrompts;
    public TextAsset textFileChoices;
    public Text textObject;
    private string[] prompts;
    private string[][] choices;

    private void Awake()
    {
        
        prompts = textFilePrompts.text.Split('\n');
        choices = new string[prompts.Length][];
        string[] choiceLines = textFileChoices.text.Split('\n');
        for (int i = 0; i < choiceLines.Length; i++)
        {
            choices[i] = choiceLines[i].Split(',');
        }
        textObject = transform.Find("PromptText").GetComponent<Text>();
        int rnd = Random.Range(0, prompts.Length);
        textObject.text = prompts[rnd];
        for (int i = 0; i < 3; i++)
        {
            Transform child;
            child = transform.Find("Choices").GetChild(i);
            child.Find("Text").GetComponent<Text>().text = choices[rnd][i];
        }
    }

}
