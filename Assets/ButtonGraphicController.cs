using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGraphicController : MonoBehaviour
{
    // Variables
    [SerializeField] private Color originalColour;

    // References 
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.transform.Find("Image").gameObject.GetComponent<Image>();
        originalColour= image.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleAlpha()
    {
        if (image.color == originalColour)
        {
            image.color = new Color(255, 255, 255, 255);
        }
        else
        {
            image.color = originalColour;
        }
    }
}
