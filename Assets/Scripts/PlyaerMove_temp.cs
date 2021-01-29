using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyaerMove_temp : MonoBehaviour
{
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if(Input.GetKey(KeyCode.W)) {
            pos.z += speed * Time.deltaTime;
            transform.position = pos;
            //CameraShake.Instance.Shake(5f, 0.1f);
        }

        if(Input.GetKey(KeyCode.S)) {
            pos.z -= speed * Time.deltaTime;
            transform.position = pos;
        }

        if(Input.GetKey(KeyCode.A)) {
            pos.x -= speed * Time.deltaTime;
            transform.position = pos;
        }

        if(Input.GetKey(KeyCode.D)) {
            pos.x += speed * Time.deltaTime;
            transform.position = pos;
        }
    }
}
