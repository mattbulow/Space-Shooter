using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    //ID for powerups 0 = tripleshot, 1 = speed up, 2 = shield
    [SerializeField] private int powerupId;

    // Update is called once per frame
    void Update()
    {
        //move down at certain speed
        transform.Translate(new Vector3(0, -_speed * Time.deltaTime, 0));
        
        //destroy object when at bottom of screen
        if (this.transform.position.y <= -6)
        {
            Destroy(this.gameObject);
        }      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                Player player = collision.gameObject.GetComponent<Player>();
                if (player != null) 
                { 
                    switch (powerupId)
                    {
                        case 0:
                            player.SetTripleShotActive();
                            break;
                        case 1:
                            player.SetSpeedUpActive();
                            break;
                        case 2:
                            player.SetShieldActive();
                            break;
                    }
                }
                Destroy(this.gameObject );
                break;
        }
    }
}
