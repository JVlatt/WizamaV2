using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Script;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private int nTilesWidth;
    
    [SerializeField]
    private  int nTilesHeight;

    [SerializeField]
    private GameObject _tilePrefab;

    [SerializeField]
    private List<Player> m_playerList;

    [SerializeField]
    private String _levelName = "Level1";

    private TilesScript[,] TileScriptsArray;

    private float _offset;

    private int m_idTiles = 0;

    public float TileSize // Calculate the tile Size
    {
        get { return _tilePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    void Awake()
    {
        _offset = _tilePrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2;
        TileScriptsArray = new TilesScript[nTilesWidth, nTilesHeight];
        CreateLevel(); //Function called to generate the level

    }

    void Update()
    {

    }

    private void CreateLevel()
    {

        Vector3 _worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)); //Set the starting point of generation to top left corner
        _worldStart.x += _offset;
        _worldStart.y -= _offset;

        //Grid is 5 by 5

        for (int y = 0; y < nTilesHeight; y++) //y position
        {

            for (int x = 0; x < nTilesWidth; x++) //x position
            {
                PlaceTile(_tilePrefab, x, y, _worldStart); //Place a tile each time this function is called
            }
        }
        //foreach (TilesScript item in TileScriptsArray)
        //  Debug.Log("Tile source d'Id : " + item.Id);
        GameManager.GetManager().InitTilesTab(TileScriptsArray, nTilesWidth, nTilesHeight);//on envoie la liste de tiles sous forme de tableau au gameManager
        GameManager.GetManager().InitGame(m_playerList);
    }

    private void PlaceTile(GameObject tilePrefab, int x, int y, Vector3 worldStart)
    {
        GameObject _tmpTile = Instantiate(tilePrefab);
        _tmpTile.GetComponent<TilesScript>().Id = m_idTiles; ;
        _tmpTile.transform.position = new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0); //Move to next position
        TileScriptsArray[x,y] = _tmpTile.GetComponent<TilesScript>();

        m_idTiles = m_idTiles + 1;
        
    }

    private string[] ReadLevel()
    {
        TextAsset _bindData = Resources.Load(_levelName) as TextAsset;
        string _data = _bindData.text.Replace(Environment.NewLine, string.Empty);
        return _data.Split('-');
    }
}
