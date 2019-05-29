using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class GameManager
    {
        #region Singleton
        static GameManager _manger = null;

        public static GameManager GetManager()
        {
            if (_manger == null)
            {
                _manger = new GameManager(); // patern singleton
            }
            return _manger;
        }
        #endregion

        #region Properties
        private List<BombeScript> m_bombList;
        public List<BombeScript> bombList
        {
            get { return m_bombList; }
            set { m_bombList = value; }
        }

        public int m_diceValue;
        public int diceValue// ceci est une propertie;
        {
            get { return m_diceValue; }
            set
            {
                m_diceValue = value;
            }
        }
        #endregion

        #region Variable

        public List<Player> m_playerList;

        public TilesScript[,] m_tilesTab;

        private List<TilesScript> _selectableTiles = new List<TilesScript>();
        private Stack<TilesScript> path = new Stack<TilesScript>(); // je sais pas encore à quoi ça sert
        private TilesScript _currentTile;
        #endregion

        #region Pathfinding
        // -------------------- ADD 22/11 (Tactical Movement)

        public TilesScript GetCurrentTile(Player player)  //Détecte la tile sur laquelle le Player est situé
        {
            foreach(TilesScript item in m_tilesTab)
            {
                if (player.transform.position == item.transform.position)
                {
                    return item;
                }
            }
            Debug.Log("Impossible de trouver la current tile");
            return null;
        }

        private void ComputeAdjacencyLists()
        {
            foreach (TilesScript tile in m_tilesTab)
            {
                tile.FindNeighbors();
            }
        }

        private void FindSelectableTiles(Player player)
        {
            ComputeAdjacencyLists();
            _currentTile = GetCurrentTile(player);

            // Start BFS

            Queue<TilesScript> _process = new Queue<TilesScript>();

            _process.Enqueue(_currentTile);
            _currentTile.visited = true;

            while(_process.Count > 0)
            {
                TilesScript t = _process.Dequeue();

                _selectableTiles.Add(t);
                t.selectableTile = true;

                if(t.distance < m_diceValue)
                {

                    foreach (TilesScript tile in t._adjacentTiles)
                    {
                        if(!tile.visited)
                        {
                            tile.parent = t;
                            tile.visited = true;
                            tile.distance = 1 + t.distance;
                            _process.Enqueue(tile);
                        }
                    }
                }
            }
        }
        #endregion

        // ---------------------   
  
        public void InitGame(List<Player> playerList)
        {
            
            m_playerList = playerList;
            Debug.Log(m_playerList.Count);
            PlayerPosition();
            m_bombList = new List<BombeScript>();
        }

        public void InitTilesTab(TilesScript[,] TilesTab, int TilesTabWidth, int TilesTabHeight)
        {
            m_tilesTab = new TilesScript[TilesTabWidth, TilesTabHeight];

            m_tilesTab = TilesTab;

            Debug.Log("Id reçu ?= 1 : " + m_tilesTab[1, 0].Id);
        }
        public void PlayerPosition()///////// à améliorer
        {
            m_playerList[0].transform.position = m_tilesTab[0,0].transform.position;
            m_playerList[0].currentTile = m_tilesTab[0,0];
            
            m_playerList[1].transform.position = m_tilesTab[13,0].transform.position;
            m_playerList[1].currentTile = m_tilesTab[13,0];
            
            m_playerList[2].transform.position = m_tilesTab[0,7].transform.position;
            m_playerList[2].currentTile = m_tilesTab[0,7];

            m_playerList[3].transform.position = m_tilesTab[13,7].transform.position;
            m_playerList[3].currentTile = m_tilesTab[13,7];
            
        }

        

        public void NextRound()//déroulement d'un tour
        {
            m_diceValue = 1;
            FindSelectableTiles(m_playerList[0]);
            FindSelectableTiles(m_playerList[1]);
            FindSelectableTiles(m_playerList[2]);
            FindSelectableTiles(m_playerList[3]);
        }

        public void MovePlayer(TilesScript tile, Player player)//appelé lorsqu'on click sur une tiles accessible par le player;
        {
            if (tile.walkableTile)
            {
                GetCurrentTile(player).walkableTile = true;
                player.transform.position = tile.transform.position;
                diceValue -= tile.distance;
                tile.walkableTile = false;
                foreach (TilesScript t in m_tilesTab)
                    t.Reset();

                FindSelectableTiles(player);
            }
        }
        public void CheckOnBomb()
        {
            List<BombeScript> copyBombList = new List<BombeScript>();
            copyBombList.AddRange(m_bombList);

            foreach(BombeScript bomb in copyBombList)
            {
                bomb.UpdateCounter();
            }
        }
    }
}
