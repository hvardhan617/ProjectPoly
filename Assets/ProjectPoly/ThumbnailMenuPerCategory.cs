using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PolyToolkit;
using UnityEngine.SceneManagement;

public class ThumbnailMenuPerCategory : MonoBehaviour {


    // public ImportAssetByAssetID importAssetByAssetID;
    public static string assetInKudan;

    //public GameObject viewInARButtonPrefab;
    //public Button backButton;

    private void Start()
    {
        Debug.Log("In start method od ThumbnailMenuPerCategory Script");
       
      
    }

    public void onViewARButtonClick()
    {
        Debug.Log(assetInKudan);
        //SceneManager.LoadScene("KudanAR");
    }

    public void getAssetByID(string name)
    {
       
        Debug.Log("Getting asset ::::" + name);
        // PolyApi.GetAsset("assets/5vbJ5vildOq", GetAssetCallback);
        PolyApi.GetAsset(name, GetAssetCallback);

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
        PolyApi.Import(result.Value, options,ImportAssetCallback);
    }

    public void ImportAssetCallback(PolyAsset asset, PolyStatusOr<PolyImportResult> result)
    {
        if (!result.Ok)
        {
            Debug.LogError("Failed to import asset. :( Reason: " + result.Status);
            return;
        }
        else
        {
            Debug.Log("Successfully imported asset!");
            assetInKudan = asset.name;
            // Here, you would place your object where you want it in your scene, and add any
            // behaviors to it as needed by your app. As an example, let's just make it
            // slowly rotate:
            result.Value.gameObject.AddComponent<Rotate>();

            
            result.Value.gameObject.SetActive(true);
            
            return;
        }
    }
}
