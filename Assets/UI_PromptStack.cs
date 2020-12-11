using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//skipped working on this for a faster junkier solution
public class UI_PromptStack : MonoBehaviour
{
    public enum Button { BUTTON_1, BUTTON_2, BUTTON_3, BUTTON_4};

    private Prompt[] prompts;
    public GameObject promptMold;

    // Start is called before the first frame update
    void Start()
    {
        prompts = new Prompt[4];


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clearPrompts()
    {
        for (int i = 0; i < prompts.Length; i++)
        {
            Destroy(prompts[i].gameObject);
            prompts[i] = null;
        }
        updatePromptPositions();
    }

    public void setPrompt(Button b, string text)
    {
        //first check to see if it exists
        if (prompts[(int)b] == null)
        {
            Debug.Log("make new");
            prompts[(int)b] = buildPrompt();
        }

        Text t = prompts[(int)b].gameObject.GetComponentInChildren<Text>();
        t.text = text;

        updatePromptPositions();
    }

    public void removePrompt(Button b)
    {
        Destroy(prompts[(int)b].gameObject);
        prompts[(int)b] = null;
        updatePromptPositions();
    }

    private void updatePromptPositions()
    {
        int i = 0;
        float height = promptMold.GetComponent<RectTransform>().rect.height;
        foreach (Prompt p in prompts)
        {
            if (p != null)
            {
                Vector3 v = new Vector3();
                v = this.gameObject.transform.position + new Vector3(0,i * height,0);
                p.gameObject.transform.position = v;
                Debug.Log(i + ": " + v.y);
                i++;
            }
        }
    }

    private Prompt buildPrompt()
    {
        Prompt p = new Prompt();
        p.gameObject = Instantiate(promptMold);
        p.gameObject.transform.SetParent(this.gameObject.transform);
        return p;
    }

    class Prompt
    {
        public GameObject gameObject;
        public Button button;
    }
}


