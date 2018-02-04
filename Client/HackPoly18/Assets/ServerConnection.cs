using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using Mapbox;
using Mapbox.Unity.Map;

[Serializable]
public struct PlaceOfInterest
{
    public string id;
    public string name;
    public float lat;
    public float lng;
    public string owner;
    public int price;
    public int income;
    public GameObject go;
}
public class ServerConnection : MonoBehaviour {
    public Mapbox.Unity.Map.BasicMap map;
    List<PlaceOfInterest> places = new List<PlaceOfInterest>();
    List<GameObject> spawnedObjs = new List<GameObject>();
    public GameObject specialHouse;
    AbstractMap _map;
    float specialDrawDistance = 0.005f;
    Mapbox.Utils.Vector2d lastMapPos = Mapbox.Utils.Vector2d.zero;
    // Use this for initialization
    void Start () {
        _map = FindObjectOfType<AbstractMap>();
            
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://hp18.herokuapp.com/properties");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            var N = JSON.Parse(www.downloadHandler.text);
            var data = N["data"].AsArray;
            foreach (JSONNode o in data)
            {
                PlaceOfInterest p = new PlaceOfInterest();
                p.id = o["id"].Value;
                p.name = o["name"].Value;
                p.lat = o["lat"].AsFloat;
                p.lng = o["lng"].AsFloat;
                p.owner = o["owner"].Value;
                p.price = o["price"].AsInt;
                p.income = o["income"].AsInt;
                p.go = null;
                print(p.name);
                places.Add(p);

                }

        }
    }

    // Update is called once per frame
    void Update () {
        //print(_map.CenterLatitudeLongitude + " " + lastMapPos);
        Mapbox.Utils.Vector2d curMapPos = _map.CenterLatitudeLongitude;
        if (!lastMapPos.Equals(curMapPos) && places.Count > 0)
        {
            //print("changed");
            lastMapPos = curMapPos;
            // draw and undraw specials
            foreach (GameObject go in spawnedObjs)
                Destroy(go.gameObject);
            spawnedObjs.Clear();
            for (int i = 0; i < places.Count; i++)
            {
                PlaceOfInterest p = places[i];

                Mapbox.Utils.Vector2d mapPos = new Mapbox.Utils.Vector2d(p.lat, p.lng);
                if (Mapbox.Utils.Vector2d.Distance(mapPos, _map.CenterLatitudeLongitude) < specialDrawDistance)
                {
                    p.go = Instantiate(specialHouse,_map.GeoToWorldPosition(new Mapbox.Utils.Vector2d(p.lat, p.lng)), Quaternion.identity);
                    Property pScript = p.go.GetComponent<Property>();
                    pScript.thisPlace = p;
                    spawnedObjs.Add(p.go);
                }
            }

        }
		
	}
}
