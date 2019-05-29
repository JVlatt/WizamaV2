using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class multipleTouch : MonoBehaviour
{
    public List<touchLocation> touches = new List<touchLocation>();
    [System.NonSerialized]
    public bool _isSetup;
    private int _itemCount = 0;
    public LayerMask _layer;
    void Update()
    {
        int i = 0;
         while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            if (t.phase == TouchPhase.Began)
            {
                touches.Add(new touchLocation(t.fingerId, AddItem(t)));
            }
            else if (t.phase == TouchPhase.Ended)
            {

                touchLocation thisTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);
                if (thisTouch != null)
                {
                    RemoveItem(thisTouch);
                }
            }
            else if (t.phase == TouchPhase.Moved)
            {
                touchLocation thisTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);

                if (thisTouch != null && thisTouch._player != null)
                {
                    RaycastHit2D hit = Physics2D.Raycast(GetTouchPosition(t.position), Vector3.forward);
                    TilesScript _targetTile = hit.transform.GetComponent<TilesScript>();
                    if (_targetTile.selectableTile)
                    {
                        thisTouch._player.Move(_targetTile);
                        GameManager.GetManager().MovePlayer(_targetTile, thisTouch._player);
                    }
                }
            }
            i++;

        }

        Vector2 GetTouchPosition(Vector2 touchPosition)
        {
            return Camera.main.ScreenToWorldPoint(touchPosition);
        }

        Player AddItem(Touch t)
        {
            RaycastHit2D hit = Physics2D.Raycast(GetTouchPosition(t.position), Vector3.forward, Mathf.Infinity, _layer);

            if (hit.transform != null)
            {
                Debug.Log(hit.transform.gameObject.name);

                if (hit.transform.GetComponent<Player>() != null)
                {
                    Player player = hit.transform.GetComponent<Player>();
                    player.m_touchId = t.fingerId;
                    _itemCount += 1;
                    return player;
                }
            }
            return null;
        }

        void RemoveItem(touchLocation touchLoc)
        {
            if (touches[touches.IndexOf(touchLoc)]._player != null)
                _itemCount -= 1;
            touches.RemoveAt(touches.IndexOf(touchLoc));
        }
    }
}
