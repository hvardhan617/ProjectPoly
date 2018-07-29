using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyToolkit;

public class Test : MonoBehaviour
{

    public GameObject itemPrefab;

    public GameObject ListItemPrefab;

    public Image img;
   
    public delegate void ListAssetsCallback(PolyStatusOr<PolyListAssetsResult> result);
    public event ListAssetsCallback listAssetsCallback;

    public delegate void FetchThumbnailCallback(PolyAsset asset, PolyStatus status);
    public event FetchThumbnailCallback fetchThumbnailCallback;

    
    private void Start()
    {
        //Texture2D blackTexture = new Texture2D(1, 1);
        //blackTexture.SetPixel(0, 0, Color.black);
        //blackTexture.Apply();
        //img.material.mainTexture = blackTexture;
        //img1.SetActive(false);
        this.fetchThumbnailCallback += handleFetchThumbnailCallback;
        this.listAssetsCallback += handleListAssetsCallback;
    }

    public void onTextureset(Texture tex)
    {
        img.material.mainTexture = tex;
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
           
            GameObject newThumbnail = (GameObject)Instantiate(itemPrefab, transform);
            //newThumbnail.tag = "";
            Button[] buttonComponents = newThumbnail.GetComponentsInChildren<Button>();
            Text[] textComponents = newThumbnail.GetComponentsInChildren<Text>();
            Image[] imgComponents = newThumbnail.GetComponentsInChildren<Image>();
            RawImage[] rawImgcomponent = newThumbnail.GetComponentsInChildren<RawImage>();

            Debug.Log("Button components::::"+ buttonComponents[0]); // shrink
           // Debug.Log("Text Components::::" + textComponents[0] + textComponents[1]); //expandtext, itemContentText
           Debug.Log("Text Components::::" + textComponents[0]);
            Debug.Log("Image Components::::" + imgComponents[0] + imgComponents[1] + imgComponents[2]);
              // Debug.Log("Image Components::::" + imgComponents[0] + imgComponents[1]+ imgComponents[2]+imgComponents[3]); // ListItem, Header, Shrink, ItemContent
              Debug.Log("Raw Image Components::::" + rawImgcomponent[0]);

           textComponents[0].text = asset.displayName;
            // textComponents[1].text = null;
            rawImgcomponent[0].material.mainTexture = asset.thumbnailTexture;
            buttonComponents[0].onClick.AddListener(() => onTextureset(asset.thumbnailTexture));


            //newThumbnail.GetComponentInChildren<RawImage>().material.mainTexture = asset.thumbnailTexture;
            ////set asset displayName as text of the button created at runtime.
            //newThumbnail.GetComponent<Button>().GetComponentInChildren<Text>().text = asset.displayName;
            //newThumbnail.name = asset.name;
            //newThumbnail.GetComponent<Button>().onClick.AddListener(() => getAssetByID(newThumbnail.name));

            newThumbnail.gameObject.SetActive(true);

        }
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

    public void onClick()
    {
        onCategorySelected("OBJECTS");
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

    public void getAssetByID(string name)
    {
        Debug.Log("BUTOON CLICKED ::::::" + name);
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


}
