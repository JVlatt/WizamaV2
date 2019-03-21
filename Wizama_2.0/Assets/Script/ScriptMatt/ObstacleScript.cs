using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

    TilesScript _obstacle;
	void Start () {
        _obstacle = GetComponent<TilesScript>();
        _obstacle.walkableTile = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
