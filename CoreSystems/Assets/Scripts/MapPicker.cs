using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPicker : MonoBehaviour {


    public List<Map> mapList;
    public int activeMap;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Map SelectMap()
    {
        int mapId = Random.RandomRange(0, mapList.Count);
        while(mapId == activeMap)
        {
            mapId = Random.RandomRange(0, mapList.Count);
        }

        mapList[activeMap].gameObject.SetActive(false);

        mapList[mapId].gameObject.SetActive(true);
        activeMap = mapId;

        return mapList[activeMap];

    }
}
