using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicData : MonoBehaviour {

    public GameObject itemPrefab;

    public void onClick()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject newItems = Instantiate<GameObject>(itemPrefab, transform);
        }
    }
}
