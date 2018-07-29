using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyToolkit;
using UnityEngine.UI;

public class ApiTest : MonoBehaviour {

	public Button listAssets;
	public Button customRequest;
	public Button getAssetByID;
	public Button importAsset;
	public Button fetchThumbnail;

    public GameObject gameObj;

  

	public delegate void FetchThumbnailCallback(PolyAsset asset,PolyStatus status);
	public event FetchThumbnailCallback fetchThumbnailCallback;

	public delegate void ImportCallback(PolyStatusOr<PolyImportResult> result);
	public event ImportCallback importCallback;

	public delegate void GetAssetCallback(PolyStatusOr<PolyAsset> result);
	public event GetAssetCallback getAssetCallbackEvent;

	public delegate void ListAssetsCallback(PolyStatusOr<PolyListAssetsResult> result);
	public event ListAssetsCallback listAssetsCallback;

	void Start(){

        gameObj.SetActive(false);

		this.fetchThumbnailCallback += handleFetchThumbnailCallback;
		this.getAssetCallbackEvent += handleGetAssetCallback;
		this.importCallback += handleImportCallback;
		this.listAssetsCallback += handleListAssetsCallback;
	}


	public void onListAssetsButtonClicked(){

		PolyApi.ListAssets(PolyListAssetsRequest.Featured(), CallListAssetsCallbackEvent);

	}
	private void CallListAssetsCallbackEvent(PolyStatusOr<PolyListAssetsResult> result){
		listAssetsCallback (result);
	}



	private void CallFetchThumbnailEvent(PolyAsset result, PolyStatus status)
    {
        Debug.Log("CallFetchThumbnailEvent::::::::::::::::::::::::::");
		if(status.ok)
		{
            Debug.Log("EVENTS::::::::::");
            //event
            fetchThumbnailCallback(result, status);
		}
	}

    public void onCustomRequestsButtonClicked() {

        PolyListAssetsRequest req = new PolyListAssetsRequest();
        // Search by keyword:
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
        req.pageSize = 20;
        // Send the request.
        //PolyApi.ListAssets(req, MyCallback);
    }



	public void onGetAssetByIDClicked(){
		PolyApi.GetAsset ("assets/8nMC2GZProF", CallAssetCallbackEvent);
	}
	private void CallAssetCallbackEvent(PolyStatusOr<PolyAsset> result){
	    if(result.Ok)
		{
			//event
			getAssetCallbackEvent (result);
		}
	}



	public void onImportAssetClicked(){
        
        //PolyApi.Import (PolyAsset asset,PolyImportOptions.Default(),CallImportCallbackEvent);
        //PolyApi.call
    }
	private void CallImportCallbackEvent(PolyStatusOr<PolyImportResult> result){
		if(result.Ok)
		{
			//event
			importCallback(result);
		}
	}


    string tagName = "Harsha";
	//event callback handlers

	void handleFetchThumbnailCallback(PolyAsset asset,PolyStatus status){

		if (status.ok) {
			Debug.Log ("handleFetchThumbnailCallback handled");
			Debug.Log (asset.thumbnail);
            gameObj.GetComponent<Renderer>().material.mainTexture = asset.thumbnailTexture;
            Debug.Log(asset.name);
            gameObj.AddComponent<Pojo>().id = asset.name.ToString();
            Debug.Log(gameObj.GetComponent<Pojo>().id);

            gameObj.SetActive(true);

        }
	}

	void handleGetAssetCallback(PolyStatusOr<PolyAsset> result){

		if (result.Ok) {
			Debug.Log ("handleGetAssetCallBack handled");
		}
	}

	void handleImportCallback(PolyStatusOr<PolyImportResult> result){

		if (result.Ok) {
			Debug.Log ("Importing the asset as Unity GameObject.");
			Debug.Log (result.Value.gameObject);
		}
	}

    void handleListAssetsCallback(PolyStatusOr<PolyListAssetsResult> result){

		if (!result.Ok) {
			Debug.Log ("Error occured while getting asset list");
		}
		Debug.Log ("Displaying thumbnails of assets retrieved");
      
        for(int i = 0; i < 1; i++)
        {
            PolyAsset res=result.Value.assets[0];
            PolyApi.FetchThumbnail(res, CallFetchThumbnailEvent);
        }

		//foreach (PolyAsset asset in result.Value.assets) {
		//	Debug.Log (asset.thumbnail); //png
  //          Debug.Log(asset.thumbnailTexture);
  //          // t = asset.thumbnailTexture;
  //          // thumbnail.GetComponent<RawImage>().texture = t;
  //          Debug.Log(asset.description);
  //          Debug.Log(asset.authorName);
  //          Debug.Log(asset.displayName);
  //          Debug.Log(asset.formats);
  //          Debug.Log(asset.name); // asset ID
  //          Debug.Log(asset.license);
  //          Debug.Log(asset.visibility);

  //          PolyApi.FetchThumbnail(asset, CallFetchThumbnailEvent);
  //      }





    }
}
