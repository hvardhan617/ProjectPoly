using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyToolkit;

public class KudanRuntimeTest : MonoBehaviour {

    public GameObject runtimeAsset;

    private void Start()
    {
        getAssetByID();
        runtimeAsset.SetActive(false);
    }

    public void getAssetByID()
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
        runtimeAsset = result.Value.gameObject;
        runtimeAsset.AddComponent<Rotate>();
    }

}
