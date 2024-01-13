using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{

    [SerializeField] int hp;
    [SerializeField] int damage;
    GameObject player;
    Skillsets playerskillsets;
    void Start()
    {
        player = GameObject.Find("Player");
        playerskillsets = player.GetComponent<Skillsets>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hit"))
        {
            hp -= playerskillsets.GetDamage();
            print(hp + " left");
            if(hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
