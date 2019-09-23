using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    public Transform target;
    public float speed;

    private Vector3 start, end;

    // Start is called before the first frame update
    void Start()
    {
        if (target != null){
            target.parent = null;
            start = transform.position;
            end = target.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            float fixedSpeed = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
        }

        if (transform.position == target.position)
        {
            target.position = (target.position == start) ? end : start;
        }
    }
}
