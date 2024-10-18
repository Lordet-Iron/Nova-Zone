using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePriceText : MonoBehaviour
{

    // Reference
    [SerializeField] private UpgradeInfo upgradeInfo;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = $"{upgradeInfo.price}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
