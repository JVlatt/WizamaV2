using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class TilesScript : MonoBehaviour {

    #region Propertie
    private bool _currentTile = false;
    public bool currentTile
    {
        get { return _currentTile; }
        set { _currentTile = value; }
    }

    private bool _targetTile = false;
    public bool targetTile
    {
        get { return _targetTile; }
        set { _targetTile = value; }
    }

    private bool _selectableTile = false;
    public bool selectableTile
    {
        get { return _selectableTile; }
        set { _selectableTile = value; }
    }

    private bool _walkableTile = true;
    public bool walkableTile
    {
        get { return _walkableTile; }
        set { _walkableTile = value; }
    }

    private bool _destroyTile = false;
    public bool destroyTile
    {
        get { return _destroyTile; }
        set { _destroyTile = value; }
    }

    public List<TilesScript> _adjacentTiles = new List<TilesScript>(); //Pas de déplacement en diagonal on va utiliser cette liste pour se déplacer

    //Les booléens liés au BFS = breadth first search (j'assimile cette notion en écrivant ça)
    private bool _visited = false; //En gros ça permet au "path finding" d'ignorer la case qu'on vient de quitter (je crois)
    public bool visited
    {
        get { return _visited; }
        set { _visited = value; }
    }

    private TilesScript _parent = null;
    public TilesScript parent
    {
        get { return _parent; }
        set { _parent = value; }
    }
    private int _distance = 0; //la variable dans laquelle on va stocker la distance entre le player et cette tile
    public int distance
    {
        get { return _distance; }
        set { _distance= value; }
    }

    //Variables hors tuto

    private int m_id;
    public int Id
    {
        get { return m_id; }
        set { m_id = value; }
    }
    #endregion

    #region Variable
    public enum TilesType
    {
        NORMAL = 0,
        WALL,
        BORDER
    }
    public TilesType m_currentTilesType = TilesType.NORMAL;///////////////////////// ENUM de la tiles

    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Sprite _normalSprite;
    private float _width;
    private Color _redColor;
    private Vector2 _size;
    public Color _exploseColor = Color.red;

    private Player _currentPlayerOnTile;
    public Player currentPlayerOnTile
    {
        get { return _currentPlayerOnTile; }
        set { _currentPlayerOnTile = value; }
    }
    #endregion

    private void Start()
    {
        _size = GetComponent<BoxCollider2D>().size;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _width = _spriteRenderer.sprite.bounds.size.x;
       
        _redColor = Color.red;
        _redColor.a = 0.80f;


    }

    private void Update()
    {
        if (_currentTile)
        { 
            _spriteRenderer.color = Color.magenta;
        }
        else if (_targetTile)
        {
            _spriteRenderer.color = Color.green;
        }
        else if (_selectableTile && _walkableTile && !destroyTile)
        {
            _spriteRenderer.color = Color.green;
        }
        else if (_destroyTile && !_selectableTile)
        {
            _spriteRenderer.color = _exploseColor;
        }
        else
        {
            _spriteRenderer.color = Color.white;
        }


    }

    public void SwitchSprite()
    {
        if (walkableTile)
            _spriteRenderer.sprite = _normalSprite;
    }

    public void Reset()
    {
        _adjacentTiles.Clear();

        _currentTile = false;
        _targetTile = false;
        _selectableTile = false;

        _visited = false;
        _parent = null;
        _distance = 0;
    }

    public void FindNeighbors()
    {
        Reset();
        CheckTile();
    }

    private void CheckTile()
    {
        Collider2D[] _colliders = Physics2D.OverlapBoxAll(transform.position, _size, 45.0f);
        
        foreach(Collider2D item in _colliders)
        {
            TilesScript tile = item.GetComponent<TilesScript>(); //check si c'est bien une tile
            if(tile != null && tile._walkableTile)
            {
                _adjacentTiles.Add(tile);
            }
        }
    }

    public void TileExplode()
    {
        if (m_currentTilesType == TilesScript.TilesType.WALL)/////////// ça doit être dans le tileScripte
        {
            m_currentTilesType = TilesScript.TilesType.NORMAL;
            walkableTile = true;
            SwitchSprite();
        }
        destroyTile = false;

        if (_currentPlayerOnTile)
            _currentPlayerOnTile.Die();
    }
}
