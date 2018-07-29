using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyToolkit;
using UnityEngine.UI;

public class FetchThumbnailTest : MonoBehaviour {

    // public Image img;
    // public RawImage img1;

    public Button b;
   

    //  public GameObject emptyObj;
    // GameObject newObj;

    public delegate void ListAssetsCallback(PolyStatusOr<PolyListAssetsResult> result);
    public event ListAssetsCallback listAssetsCallback;

    public delegate void FetchThumbnailCallback(PolyAsset asset, PolyStatus status);
    public event FetchThumbnailCallback fetchThumbnailCallback;

    private void Awake()
    {
        //emptyObj.SetActive(false);
    }

    private void Start()
    {
       
        //img1.SetActive(false);
        this.fetchThumbnailCallback += handleFetchThumbnailCallback;
        this.listAssetsCallback += handleListAssetsCallback;
    }

    public void onClick()
    {
        onCategorySelected("OBJECTS");
    }

    void handleFetchThumbnailCallback(PolyAsset asset, PolyStatus status)
    {

        if (status.ok)
        {
            Debug.Log("handleFetchThumbnailCallback handled");
            Debug.Log(asset.thumbnail);
            //RawImage newObj = (RawImage)Instantiate(img1,transform);
            //// newObj.SetActive(false);

            //Debug.Log(asset.name);
            //newObj.material.mainTexture = asset.thumbnailTexture;
            ////newObj.SetNativeSize();
            Button newObj;
            newObj = (Button)Instantiate(b, transform);
           // newObj.AddComponent<ObjectClick>()=newObj;
            newObj.GetComponentInChildren<RawImage>().material.mainTexture = asset.thumbnailTexture;
            newObj.GetComponentInChildren<Text>().text = asset.displayName;
            newObj.name = asset.name;
            newObj.onClick.AddListener(() => getAssetByID(newObj.name));



            newObj.gameObject.SetActive(true);

        }
    }

    public void getAssetByID(string name)
    {
        Debug.Log("BUTOON CLICKED ::::::"+name);
        PolyApi.GetAsset("assets/5vbJ5vildOq", GetAssetCallback);

    }

    // Callback invoked when the featured assets results are returned.
    private void GetAssetCallback(PolyStatusOr<PolyAsset> result)
    {
        if (!result.Ok)
        {
            Debug.LogError("Failed to get assets. Reason: " + result.Status);
            return;
        }
        Debug.Log("Successfully got asset!");

        // Set the import options.
        PolyImportOptions options = PolyImportOptions.Default();
        // We want to rescale the imported mesh to a specific size.
        options.rescalingMode = PolyImportOptions.RescalingMode.FIT;
        // The specific size we want assets rescaled to (fit in a 5x5x5 box):
        options.desiredSize = 5.0f;
        // We want the imported assets to be recentered such that their centroid coincides with the origin:
        options.recenter = true;
        PolyApi.Import(result.Value, options, ImportAssetCallback);
    }

    private void ImportAssetCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
    {
        if (!result.Ok)
        {
            Debug.LogError("Failed to import asset. :( Reason: " + result.Status);
            return;
        }
        Debug.Log("Successfully imported asset!");

        // Here, you would place your object where you want it in your scene, and add any
        // behaviors to it as needed by your app. As an example, let's just make it
        // slowly rotate:
        result.Value.gameObject.AddComponent<Rotate>();
    }

    void handleListAssetsCallback(PolyStatusOr<PolyListAssetsResult> result)
    {

        if (!result.Ok)
        {
            Debug.Log("Error occured while getting asset list");
        }
        Debug.Log("Displaying thumbnails of assets retrieved");

        //for (int i = 0; i < 1; i++)
        foreach (PolyAsset asset in result.Value.assets)
        {
             //PolyAsset res = result.Value.assets[1];
            PolyApi.FetchThumbnail(asset, CallFetchThumbnailEvent);
        }
    }

    private void CallFetchThumbnailEvent(PolyAsset result, PolyStatus status)
    {
        Debug.Log("CallFetchThumbnailEvent::::::::::::::::::::::::::");
        if (status.ok)
        {
            Debug.Log("EVENTS::::::::::");
            //event
            fetchThumbnailCallback(result, status);
        }
    }

    public void onCategorySelected(string category)
    {
        Debug.Log(category + "::::" + PolyCategory.TRANSPORT);
        PolyListAssetsRequest req = new PolyListAssetsRequest();
        // Search by keyword:
        req.category = PolyCategory.OBJECTS;
        //req.keywords = "tree";
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
        PolyApi.ListAssets(PolyListAssetsRequest.Featured(), CallListAssetsCallbackEvent);

    }
    private void CallListAssetsCallbackEvent(PolyStatusOr<PolyListAssetsResult> result)
    {
        listAssetsCallback(result);
    }

}
