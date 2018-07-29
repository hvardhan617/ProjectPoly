using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyToolkit;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ThumbnailController : MonoBehaviour
{

    //button Prefab that will be used to create runtime transparent buttons that act as holders for thumbnail textures.
    public GameObject buttonPrefab;
    //needed to call Poly API in next ImportMenu
    public ThumbnailMenuPerCategory thumbnailMenuPerCategory;
    //back button to go back to category Menu
    public Button backbutton;
    //Backgound Image in Category Menu.Along with this,there is a Text element in UI saying "Select a Category".
    public GameObject backGroundImage;
    //Background Image for Thumbnail Menu(to be removed as backgroundImage is itself enuf).
    public GameObject thumbnailMenuBackgroundImage;
    public GameObject modelbackGroundImg;

    public GameObject viewInARButtonPrefab;
    public Text selectedCategoryName;
    public delegate void ListAssetsCallback(PolyStatusOr<PolyListAssetsResult> result);
    public event ListAssetsCallback listAssetsCallback;

    public delegate void FetchThumbnailCallback(PolyAsset asset, PolyStatus status);
    public event FetchThumbnailCallback fetchThumbnailCallback;


    private void Start()
    {
       // thumbnailMenuPerCategory = new ThumbnailMenuPerCategory();
        this.fetchThumbnailCallback += handleFetchThumbnailCallback;
        this.listAssetsCallback += handleListAssetsCallback;

    }

    void handleFetchThumbnailCallback(PolyAsset asset, PolyStatus status)
    {

        if (status.ok)
        {
            Debug.Log("handleFetchThumbnailCallback handled");
            Debug.Log(asset.thumbnail);

            GameObject newThumbnail = (GameObject)Instantiate(buttonPrefab, transform);
            //newThumbnail.tag = "";
            Button[] buttonComponents = newThumbnail.GetComponentsInChildren<Button>();
            Text[] textComponents = newThumbnail.GetComponentsInChildren<Text>();
            Image[] imgComponents = newThumbnail.GetComponentsInChildren<Image>();
            RawImage[] rawImgcomponent = newThumbnail.GetComponentsInChildren<RawImage>();

            Debug.Log("Button components::::" + buttonComponents[0]); // shrink
                                                                      // Debug.Log("Text Components::::" + textComponents[0] + textComponents[1]); //expandtext, itemContentText
            Debug.Log("Text Components::::" + textComponents[0]);
            Debug.Log("Image Components::::" + imgComponents[0] + imgComponents[1] + imgComponents[2]);
            // Debug.Log("Image Components::::" + imgComponents[0] + imgComponents[1]+ imgComponents[2]+imgComponents[3]); // ListItem, Header, Shrink, ItemContent
            Debug.Log("Raw Image Components::::" + rawImgcomponent[0]);

            textComponents[0].text = asset.displayName;
            // textComponents[1].text = null;
            rawImgcomponent[0].material.mainTexture = asset.thumbnailTexture;
            rawImgcomponent[0].name = asset.name;
            rawImgcomponent[0].gameObject.AddComponent<ThumbnailMenuPerCategory>();
            rawImgcomponent[0].gameObject.AddComponent<ClickImg>().thumbnailMenuBackgroundImg=this.thumbnailMenuBackgroundImage;
            rawImgcomponent[0].gameObject.AddComponent<ClickImg>().modelbackGroundImg = this.modelbackGroundImg;
           

            newThumbnail.gameObject.SetActive(true);

        }
    }

    void handleListAssetsCallback(PolyStatusOr<PolyListAssetsResult> result)
    {
        Debug.Log(result.Status);
       
        if (!result.Ok)
        {
            Debug.Log("Error occured while getting asset list");
            return;
        }
        Debug.Log("Displaying thumbnails of assets retrieved");

       //  for (int i = 0; i < 3; i++)
        foreach (PolyAsset asset in result.Value.assets)
        {
           // PolyAsset res = result.Value.assets[i];
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
        selectedCategoryName.text = category;
        Debug.Log("Categrou Nmae::::::"+selectedCategoryName.text);
        selectedCategoryName.gameObject.SetActive(true);
        PolyListAssetsRequest req = new PolyListAssetsRequest();

        if(category == PolyCategory.ANIMALS.ToString())
        {
            req.category = PolyCategory.ANIMALS;
        }
        else if(category == PolyCategory.ARCHITECTURE.ToString())
        {
            req.category = PolyCategory.ARCHITECTURE;
        }
        else if (category == PolyCategory.ART.ToString())
        {
            req.category = PolyCategory.ART;
        }
        else if (category == PolyCategory.FOOD.ToString())
        {
            req.category = PolyCategory.FOOD;
        }
        else if (category == PolyCategory.NATURE.ToString())
        {
            req.category = PolyCategory.NATURE;
        }
        else if (category == PolyCategory.OBJECTS.ToString())
        {
            req.category = PolyCategory.OBJECTS;
        }
        else if (category == PolyCategory.PEOPLE.ToString())
        {
            req.category = PolyCategory.PEOPLE;
        }
        else if (category == PolyCategory.PLACES.ToString())
        {
            req.category = PolyCategory.PLACES;
        }
        else if (category == PolyCategory.TECH.ToString())
        {
            req.category = PolyCategory.TECH;
        }
        else if (category == PolyCategory.TRANSPORT.ToString())
        {
            req.category = PolyCategory.TRANSPORT;
        }
        else
        {
            req.category = PolyCategory.UNSPECIFIED;
        }
        // Search by keyword:
        //req.category = PolyCategory.TRANSPORT;
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
        //PolyApi.ListAssets(PolyListAssetsRequest.Featured(), CallListAssetsCallbackEvent);
        PolyApi.ListAssets(req, CallListAssetsCallbackEvent);

    }
    private void CallListAssetsCallbackEvent(PolyStatusOr<PolyListAssetsResult> result)
    {
        listAssetsCallback(result);
    }

    public void onBackButtonClick()
    {
        thumbnailMenuBackgroundImage.SetActive(false);
        backGroundImage.SetActive(true);
    }

   
}

