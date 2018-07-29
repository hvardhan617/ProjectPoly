using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyToolkit;
using UnityEngine.UI;

public class Poc : MonoBehaviour {

    public GameObject itemPrefab;
    public GameObject scrollContainer;

    public Button b;

    public void onButtonClick()
    {
        PolyApi.ListAssets(PolyListAssetsRequest.Featured(), InitialiseUIContent);
    }

    void InitialiseUIContent(PolyStatusOr<PolyListAssetsResult> result)
    {
        foreach(PolyAsset asset in result.Value.assets)
        {
            PolyApi.FetchThumbnail(asset, callback);
           
        }
    }

	void CreateItems(PolyAsset asset)
    {
        
        GameObject newItem = Instantiate(itemPrefab) as GameObject;
        newItem.GetComponent<Item>().CreateEachItem(asset);
        newItem.transform.SetParent(scrollContainer.transform, false);
    }

    void callback(PolyAsset asset, PolyStatus status)
    {

        if (status.ok)
        {
            CreateItems(asset);
        }
    }
}
