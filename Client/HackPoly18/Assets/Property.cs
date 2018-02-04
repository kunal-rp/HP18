using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : MonoBehaviour {
    public PlaceOfInterest thisPlace;

    GameObject propertyCanvas;
	// Use this for initialization
	void Start () {
        


    }

    public void OnMouseDown()
    {
        propertyCanvas = Camera.main.GetComponent<CanvasReferences>().propertyCanvas.gameObject;
        propertyCanvas.SetActive(true);
        PropertyCanvas canvasScript = propertyCanvas.GetComponent<PropertyCanvas>();
        canvasScript.nameTxt.text = thisPlace.name;
        canvasScript.costTxt.text = thisPlace.price.ToString();
        canvasScript.ownerTxt.text = thisPlace.owner;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
