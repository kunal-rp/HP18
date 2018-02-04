using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using Mapbox;
using Mapbox.Unity.Map;

[Serializable]
struct PlaceOfInterest
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

    List<int> indicesToDelete = new List<int>();
    // Update is called once per frame
    void Update () {

        if(lastMapPos != _map.CenterLatitudeLongitude  && places.Count > 0)
        {
            // draw and undraw specials
            indicesToDelete.Clear();
            for (int i = 0; i < places.Count; i++)
            {
                PlaceOfInterest p = places[i];
                Mapbox.Utils.Vector2d mapPos = new Mapbox.Utils.Vector2d(p.lat, p.lng);
                if (Mapbox.Utils.Vector2d.Distance(mapPos, _map.CenterLatitudeLongitude) < specialDrawDistance)
                {
                    if(p.go != null)
                    {
                        Destroy(p.go);
                    }
                    //Conversions.GeoToWorldPosition(convert, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz()
                    // p.go = Instantiate(specialHouse, Mapbox.Unity.Utilities.VectorExtensions.AsUnityPosition(new Vector2(p.lat, p.lng), map.CenterMercator, _map.world), Quaternion.identity);
                   p.go = Instantiate(specialHouse,_map.GeoToWorldPosition(new Mapbox.Utils.Vector2d(p.lat, p.lng)), Quaternion.identity);
                    print("Spawn");
                }
                else if (p.go != null)
                {
                    indicesToDelete.Add(i);
                }
            }
            for(int i = 0; i < indicesToDelete.Count;i++)
            {
                Destroy(places[indicesToDelete[i]].go.gameObject);
                print("destroy");
            }
            indicesToDelete.Clear();
            lastMapPos = _map.CenterLatitudeLongitude;
        }
		
	}
}
