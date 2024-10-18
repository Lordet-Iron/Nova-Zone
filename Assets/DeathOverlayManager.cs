using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathOverlayManager : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.O))
        {
            if (gameObject.GetComponent<TextMeshProUGUI>().enabled)
            {
                gameObject.GetComponent<TextMeshProUGUI>().enabled = false;
            }
        }
    }
}
