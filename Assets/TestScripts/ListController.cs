using System.Collections;
using UnityEngine;
using PolyToolkit;

public class ListController : MonoBehaviour
{

    // public Sprite[] AnimalImages;
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;

  

    void Start()
    {

        // 1. Get the data to be displayed
        //       Anmimals = new ArrayList(){
        //new Animal(AnimalImages[0],
        //           "Cat",
        //           "Power\t:\t5\nAttack\t:\t5\nTame\t:\t10\nVenom\t:\t0"),
        //new Animal(AnimalImages[1],
        //           "Dog",
        //           "Power\t:\t5\nAttack\t:\t5\nTame\t:\t10\nVenom\t:\t0"),
        //new Animal(AnimalImages[2],
        //           "Fish",
        //           "Power\t:\t5\nAttack\t:\t5\nTame\t:\t10\nVenom\t:\t0"),
        //new Animal(AnimalImages[3],
        //           "Parrot",
        //           "Power\t:\t5\nAttack\t:\t5\nTame\t:\t10\nVenom\t:\t0"),
        //new Animal(AnimalImages[4],
        //           "Rabbit",
        //           "Power\t:\t5\nAttack\t:\t5\nTame\t:\t10\nVenom\t:\t0"),
        //new Animal(AnimalImages[5],
        //           "Snail",
        //           "Power\t:\t5\nAttack\t:\t5\nTame\t:\t10\nVenom\t:\t0")
        //};

        // 2. Iterate through the data, 
        //	  instantiate prefab, 
        //	  set the data, 
        //	  add it to panel



        onCategorySelected("TRANSPORT");




    }

    public void onCategorySelected(string category)
    {
        Debug.Log(category + "::::" + PolyCategory.TRANSPORT);
        PolyListAssetsRequest req = new PolyListAssetsRequest();
        // Search by keyword:
        req.category = PolyCategory.TRANSPORT;
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
        PolyApi.ListAssets(req, CallListAssetsCallbackEvent);

    }

    private void CallListAssetsCallbackEvent(PolyStatusOr<PolyListAssetsResult> result)
    {
        foreach (PolyAsset asset in result.Value.assets)
        {
            PolyApi.FetchThumbnail(asset, callback);
           
        }

    }

    void callback(PolyAsset asset, PolyStatus status)
    {
        GameObject newAnimal = Instantiate(ListItemPrefab) as GameObject;
        ListItemController controller = newAnimal.GetComponent<ListItemController>();

        controller.Icon.GetComponent<Renderer>().material.mainTexture = asset.thumbnailTexture;
        controller.Name.GetComponent<TextMesh>().text = asset.displayName;

        newAnimal.transform.parent = ContentPanel.transform;
        newAnimal.transform.localScale = Vector3.one;
    }


}