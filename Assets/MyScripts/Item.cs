using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyToolkit;

public class Item : MonoBehaviour {

   // public Text displayName;
    //public Text description;
    public GameObject item;
    public GameObject dispName;
  

    public void CreateEachItem(PolyAsset asset)
    {
        // this.displayName.text = asset.displayName;
        Debug.Log(asset.displayName);
        Debug.Log(dispName);
        Debug.Log(item);
        item = new GameObject();
        dispName = new GameObject();
        Debug.Log(dispName);
        Debug.Log(item);
        //Debug.Log(dispName.GetComponent<Text>().text);
        dispName.AddComponent<Text>().text=asset.displayName;
       // dispName.GetComponent<Text>().text = asset.displayName;
        Debug.Log(dispName.GetComponent<Text>().text);
       

        item.AddComponent<MeshRenderer>().material.mainTexture=asset.thumbnailTexture;
       // item.GetComponent<Renderer>().material.mainTexture = asset.thumbnailTexture;
   
    }
}
