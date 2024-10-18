using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradePreviewManager : MonoBehaviour
{

    // References
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private UnityEngine.UI.Image preview_Image;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshPreview(UpgradeInfo upgrade)
    {
        name.text = upgrade.name;
        description.text = upgrade.description;
        preview_Image.sprite = upgrade.previewImageSprite;
    }
}
