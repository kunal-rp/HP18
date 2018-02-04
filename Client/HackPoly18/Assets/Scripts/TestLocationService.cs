using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;
using Mapbox.Examples;
using Mapbox.Unity.Map;

public class TestLocationService : MonoBehaviour
{
    AbstractMap _map;
    public ForwardGeocodeUserInput geo;
    private Vector2 coords = Vector2.zero;
    public ReloadMap reloadScr;

    public Vector2 Coords
    {
        get
        {
            return coords;
        }

        set
        {
            coords = value;
        }
    }

    public void Start()
    {
        _map = FindObjectOfType<AbstractMap>();
        StartCoroutine("StartCo");
    }
    IEnumerator StartCo()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            if (Input.location.status == LocationServiceStatus.Running)
            {
                // Access granted and location value could be retrieved
                print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
                coords.x = Input.location.lastData.latitude;
                coords.y = Input.location.lastData.longitude;
                //geo.HandleUserInput(coords.x + ", " + coords.y);
                _map.SetCenterLatitudeLongitude(new Mapbox.Utils.Vector2d(coords.x, coords.y));
                _map.Initialize(_map.CenterLatitudeLongitude, _map.AbsoluteZoom);
                //reloadScr.
                // yield return new WaitForSeconds(4f);
                yield return null;
            }
            //Start();
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
        yield return null;
    }
}