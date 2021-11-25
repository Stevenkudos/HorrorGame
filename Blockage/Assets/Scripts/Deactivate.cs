using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivate : MonoBehaviour
{
    public GameObject plank1;
    public GameObject plank2;

    public GameObject BlockHint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!plank1.GetComponent<Rigidbody>().isKinematic && !plank2.GetComponent<Rigidbody>().isKinematic)
        {
            if (BlockHint!=null)
            {
                BlockHint.SetActive(false);
            }
            
            GetComponent<Rigidbody>().isKinematic = false;
            StartCoroutine(ClearPlanks());
        }
    }

    private IEnumerator ClearPlanks()
    {
        yield return new WaitForSeconds(2f);
        plank1.SetActive(false);
        plank2.SetActive(false);
    }
}
