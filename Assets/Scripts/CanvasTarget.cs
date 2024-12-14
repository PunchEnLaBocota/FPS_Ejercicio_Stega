using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTarget : MonoBehaviour
{
    public float d;
    public Transform target;
   
    void Update()
    {
        //Que mire a la c�mara
        transform.LookAt(Camera.main.transform);
        //Si target no es nulo
        if(target != null)
        //Coge la posici�n del target y sube arriba la distancia que le hemos
        //puesto en d, no va a rotar junto con el target porque no es hijo del cubo/target.
        transform.position = new Vector3(target.position.x, target.position.y + d,
            target.position.z);
    }
}
