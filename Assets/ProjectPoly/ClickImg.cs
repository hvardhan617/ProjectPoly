using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickImg : MonoBehaviour, IPointerClickHandler
{
    public GameObject modelbackGroundImg;
    public GameObject thumbnailMenuBackgroundImg;


    public void OnPointerClick(PointerEventData eventData)
    {
        try
        {
            modelbackGroundImg.SetActive(true);
            thumbnailMenuBackgroundImg.SetActive(false);
           
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
        this.gameObject.GetComponent<ThumbnailMenuPerCategory>().getAssetByID(this.gameObject.name);
        //Debug.Log("Image Clicked :::::" + eventData.selectedObject.name);
    }
}
