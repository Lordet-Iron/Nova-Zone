using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{

    // References
    [SerializeField] private GameObject UI;
    private GameObject Victory;
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.FindGameObjectWithTag("UI");
        Victory = UI.transform.Find("Victory").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (gameObject.GetComponent<Enemy>().health <= 0)
        {
            Debug.Log("dead");
            Victory.SetActive(true);
        }
        
    }
}
