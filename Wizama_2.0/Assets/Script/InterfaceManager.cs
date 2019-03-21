using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Script;

public class InterfaceManager : MonoBehaviour {

    public GameObject Bomb;
    [SerializeField]
    private GameObject winScreen;
    [SerializeField]
    private Text winText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckTheWin();
	}

    public void PlaceBomb()
    {
        Instantiate(Bomb, GameManager.GetManager().currentPlayer.transform.position, Quaternion.identity);
        GameManager.GetManager().GetCurrentTile(GameManager.GetManager().currentPlayer).walkableTile = false;
    }

    public void EndTurn()
    {
        GameManager.GetManager().diceValue = 0;
    }

    public void CheckTheWin()
    {
        if (GameManager.GetManager().m_playerList.Count == 1)
        {
            winScreen.SetActive(true);
            winText.text = "Le Joueur " + (GameManager.GetManager().currentPlayer.m_playerId + 1).ToString() + " gagne !!!";
            StartCoroutine(RestartGame());
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}