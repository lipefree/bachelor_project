using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParentButton : MonoBehaviour
{   
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
    }

    public void setText(string text) 
    {
        this.text.text = text;
    }
}
