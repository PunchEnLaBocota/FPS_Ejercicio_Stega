using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shooting : MonoBehaviour
{
    public float damage;
    public float range;
    public float impactForce;
    public float timeBetweenBullets;//Cadencia del arma
    public ParticleSystem muzzleEffects;

    Camera cam;
    AudioSource audioSource;
    Ray ray;
    RaycastHit hit;
    float timer;//En los timer no usar Coroutinas para las armas, hacer timer normal

    Target target;


    private void Awake()
    {
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        SelectTarget();
        //Contador de tiempo
        timer += Time.deltaTime;
        //Si pulso el boton izquierdo del ratón y ha pasado el tiempo suficiente, disparo
        if (Input.GetMouseButtonDown(0) && timer >= timeBetweenBullets)
            Shoot();
    }

    void Shoot()
    {
        //Reseteamos el timer
        timer = 0;
        //Reproduzco mi sistema de particulas
        muzzleEffects.Play();
        //Reproduzco mi audio
        audioSource.Play();

        //el origen del raycast va a estar, en la posición de la cámara con
        //la dirección de la cámara
        ray.origin = cam.transform.position;
        ray.direction = cam.transform.forward;

        if(Physics.Raycast(ray, out hit, range))
        {
            //Si choca con el algo que tenga el script/componente Target
            Target target = hit.transform.GetComponent<Target>();
            
            //Si target no es nulo, es decir choca con el target
            if (target != null)
            {
                //Si este no es mi target seleccionado
                if (GameManager.gameManager.CheckTarget(target) == false)
                {

                    //Llamamos a SelectTarget para seleccionarlo
                    GameManager.gameManager.SelectTarget(target);

                }
              
   
                //Llamamos a la función target, el target está haciendo referencia
                //a nuestra clase Target del Script Target, que tiene una función
                //pública que es TakeDamage
                target.TakeDamage(damage);
                //Posicionamos el efecto de particulas, justo en el punto de impacto
                //del raycast con el cubo/target 
                target.hitEffect.transform.position = hit.point;
                //Lo reproduzco
                target.hitEffect.Play();

                //El objeto al que he disparado si tiene rigidbody
                if(hit.rigidbody != null)
                    //Le añadimos fuerza, hit.normal es la normal del raycast es decir,
                    //es una fuerza que va hacia el punto donde has disparado,
                    //se acercaría el target a nosotros al dispararle. Le ponemos
                    //-hit.normal para darle fuerza hacia la dirección
                    //que estoy apuntando, que se aleje de nosotros
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }
    void SelectTarget()
    {
        
        ray.origin = cam.transform.position;
        ray.direction = cam.transform.forward;

        if (Physics.Raycast(ray, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
           
            if (target != null)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    //Llamamos al método SelectTarget del script GameManager
                    GameManager.gameManager.SelectTarget(target);
                }
            }
        }
    }
}
