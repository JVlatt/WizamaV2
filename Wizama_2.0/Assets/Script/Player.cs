using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class Player : MonoBehaviour {

    [SerializeField]
    public int m_playerId;

    private TilesScript m_currentTile;
    public TilesScript currentTile
    {
        get { return m_currentTile; }
        set { m_currentTile = value; }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Talk()
    {
        Debug.Log("Je m'appelle : " + gameObject.name);
    }

    public void Move(TilesScript newCurrentTiles)
    {
        //Debug.Log("Tile de base : " + m_currentTile.Id + " et la nouvelle Tile : " + newCurrentTiles.Id);
        m_currentTile.currentPlayerOnTile = null;
        m_currentTile = newCurrentTiles;
        m_currentTile.currentPlayerOnTile = this;
    }

    public void Die()
    {
        Debug.Log(gameObject.name + " est mort normalement");
        GameManager.GetManager().m_playerList.Remove(this);
        Destroy(this.gameObject);
    }
}