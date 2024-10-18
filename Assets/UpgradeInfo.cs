using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeInfo : MonoBehaviour
{
    // Variables
    [SerializeField] public string name;
    [SerializeField] public string description;
    [SerializeField] public string type; // Types: Health
    [SerializeField] public int modif; // Strength
    [SerializeField] public int price = 0;
    [SerializeField] public bool isPurchased = false;
    [SerializeField] public Sprite previewImageSprite;
    private Transform transform;



    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        Transform childTransform = transform.Find("Image");
        previewImageSprite = childTransform.GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
