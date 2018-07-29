using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PolyToolkit;

public class TouchableGameObject : MonoBehaviour {

     GameObject gameObj;

    public GameObject assetPrefab;
    public GameObject panel;
    public GameObject currentpanel;

	// Use this for initialization
	void Start () {
       panel.SetActive(false);


    }

    public void onCategorySelected(string category)
    {
        Debug.Log(category + "::::" + PolyCategory.TRANSPORT);
            PolyListAssetsRequest req = new PolyListAssetsRequest();
            // Search by keyword:
            req.category = PolyCategory.TRANSPORT;
            req.keywords = "tree";
            // Only curated assets:
            req.curated = true;
            // Limit complexity to medium.
            req.maxComplexity = PolyMaxComplexityFilter.MEDIUM;
            // Only Blocks objects.
            req.formatFilter = PolyFormatFilter.BLOCKS;
            // Order from best to worst.
            req.orderBy = PolyOrderBy.BEST;
            // Up to 20 results per page.
            //req.pageSize = 20;
            PolyApi.ListAssets(req, CallListAssetsCallbackEvent);
                 
    }

    private void CallListAssetsCallbackEvent(PolyStatusOr<PolyListAssetsResult> result)
    {
        foreach (PolyAsset asset in result.Value.assets)
        {
            PolyApi.FetchThumbnail(asset, callback);

        }

        currentpanel.SetActive(false);
        panel.SetActive(true);
       // SceneManager.LoadScene("2");
       
    }

    void callback(PolyAsset asset, PolyStatus status)
    {

        if (status.ok)
        {
            CreateItems(asset);
        }
    }

    void CreateItems(PolyAsset asset)
    {
        

        GameObject newItem = Instantiate<GameObject>(assetPrefab, assetPrefab.transform);
        newItem.AddComponent<Item>().CreateEachItem(asset);
        //newItem.GetComponent<Item>().CreateEachItem(asset);
       //    newItem.transform.SetParent(scrollContainer.transform, false);
    }


    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray,out hit))
            {
                if(hit.transform.name == gameObj.name)
                {

                    SceneManager.LoadScene("3");
                    
                }
            }
        }
	}
}
