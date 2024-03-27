using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // speed variable = 8
    [SerializeField] private float _speed = 8f;

    // Update is called once per frame
    void Update()
    {
        //translate up forever
        transform.Translate(new Vector3(0,_speed*Time.deltaTime,0));

        // need to delete laser when its out of frame
        if (transform.position.y > 8) 
        {
            Transform parent = this.transform.parent;
            if (parent != null)
            {
                Destroy(parent.gameObject);
            } else
            {
                Destroy(this.gameObject);
            }
            
        }
    }
}
