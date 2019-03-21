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
                if (m_diceValue == 0)
                    ChangePlayerRound();//changement de tour quand le compteur du dé est à 0;
            }
        }
        #endregion

        #region Variable
        public enum PlayerRound
        {
            P1 = 0,
            P2,
            P3,
            P4
        }
        public PlayerRound m_currentPlayerRound = PlayerRound.P1;

        public List<Player> m_playerList;
        public List<PlayerRound> m_PlayerRoundList;
        private int IndexRound;

        public Player m_currentPlayer;
        public Player currentPlayer
        {
            get { return m_currentPlayer; }
            set { m_currentPlayer = value; }
        }


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

        private void FindSelectableTiles()
        {
            ComputeAdjacencyLists();
            _currentTile = GetCurrentTile(m_currentPlayer);

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
            m_currentPlayer = m_playerList[0];
            NextRound();
            m_bombList = new List<BombeScript>();

            m_PlayerRoundList = new List<PlayerRound>();
            m_PlayerRoundList.Add(PlayerRound.P1);
            m_PlayerRoundList.Add(PlayerRound.P2);
            m_PlayerRoundList.Add(PlayerRound.P3);
            m_PlayerRoundList.Add(PlayerRound.P4);

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

        public void ChangePlayerRound() // meilleur maintenant
        {
            CheckOnBomb();

            IndexRound += 1;
            if (IndexRound > m_playerList.Count - 1)
                IndexRound = 0;

            m_currentPlayer = m_playerList[IndexRound];
            //m_currentPlayerRound = m_PlayerRoundList[IndexRound];
            NextRound();
        }

        public void NextRound()//déroulement d'un tour
        {
            m_diceValue = Random.Range(1, 6);
            FindSelectableTiles();
        }

        public void MovePlayer(GameObject tile)//appelé lorsqu'on click sur une tiles accessible par le player;
        {
            if (tile.GetComponent<TilesScript>().walkableTile)
            {
                GetCurrentTile(m_currentPlayer).walkableTile = true;
                m_currentPlayer.transform.position = tile.transform.position;
                TilesScript tileObject = tile.GetComponent<TilesScript>();
                diceValue -= tileObject.distance;
                tileObject.walkableTile = false;
                foreach (TilesScript t in m_tilesTab)
                    t.Reset();

                FindSelectableTiles();
            }
        }

        public void EndTurn()
        {
            ChangePlayerRound();
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
