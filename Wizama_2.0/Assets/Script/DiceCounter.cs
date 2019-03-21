using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;

public class DiceCounter : MonoBehaviour {

    private Text m_diceValueText;

	// Use this for initialization
	void Start ()
    {
        m_diceValueText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_diceValueText.text = GameManager.GetManager().m_diceValue.ToString();
	}
}
