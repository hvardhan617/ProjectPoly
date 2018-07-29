using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryListController : MonoBehaviour {

    //category button prefabs that will be initialized at runtime. 
    public GameObject prefab; 
    ArrayList categories;
    //required to call Poly API when any category button is clicked.
     public ThumbnailController thumbnailController;
    //Backgound Image in Category Menu.Along with this,there is a Text element in UI saying "Select a Category".
    public GameObject backGroundImage;
    //Background Image for Thumbnail Menu(to be removed as backgroundImage is itself enuf).
    public GameObject thumbnailMenuBackgroundImage;

   

    // public int numberToCreate; // number of objects to create. Exposed in inspector

    void Start()
    {
        //Initialize PolyAPI
        PolyToolkit.PolyApi.Init();
  
        categories = new ArrayList()
        {
            new PolyCategories("ART"),
            new PolyCategories("ARCHITECTURE"),
            new PolyCategories("FURNITURE & HOME"),
            new PolyCategories("PEOPLE & CHARACTER"),
            new PolyCategories("OBJECTS"),
            new PolyCategories("TRANSPORT"),
            new PolyCategories("TOOLS & TECHNOLOGY"),
            new PolyCategories("TRAVEL & LEISURE")
        };
       
        Populate();
        backGroundImage.SetActive(true);
       
    }

    //function to create category Buttons as runtime.
    void Populate()
    {

        foreach (PolyCategories category in categories)
        {
            GameObject newObj = new GameObject();
            Debug.Log(category.categoryName);
            newObj = (GameObject)Instantiate(prefab, transform);
            newObj.name = category.categoryName;
            newObj.GetComponent<Button>().GetComponentInChildren<Text>().text = category.categoryName;
            Debug.Log(newObj.GetComponent<Button>().GetComponentInChildren<Text>().text);
            newObj.GetComponent<Button>().onClick.AddListener(() => onCategoryButtonClick(newObj.name));
        }
         
    }

    public void onCategoryButtonClick(string category)
    {
        Debug.Log(category + "button clicked");
        backGroundImage.SetActive(false);
        thumbnailMenuBackgroundImage.SetActive(true);
        thumbnailController.onCategorySelected(category);
    }
}
