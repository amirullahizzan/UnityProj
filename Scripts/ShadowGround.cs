using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ShadowGround : MonoBehaviour
{
    // Start is called before the first frame update
    Transform playerPos;
    SpriteRenderer sprite;

    void Start()
    {
        playerPos = transform.parent.GetChild(0).GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        raycastOrigin = playerPos.position + Vector3.up * 0.3f;
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, Mathf.Infinity, groundLayerMask, QueryTriggerInteraction.Ignore) )
        {
            sprite.enabled = true;
            PlaceShadow(hit.point);
        }
        else
        {
            sprite.enabled = false;
        }

    }

    Vector3 raycastOrigin;
    public LayerMask groundLayerMask;
    void Update()
    {
      
    }

    void PlaceShadow(Vector3 hitpos)
    {
        hitpos += Vector3.down * 0.1f;
        transform.position = new Vector3(transform.position.x, hitpos.y, transform.position.z);
    }
}
