using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonBrick : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            transform.position = new Vector3(Random.Range(-3, 3), transform.position.y, Random.Range(-3, 3));
        }
    }
}
