using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Hacemos un #region al principio y un #endregion para agrupar el codigo
    //y poder ocultarlo
    #region Public_Variables
    [Header("Movement")]
    public float accelerationSpeed;
    public float desaccelerationSpeed;
    public int maxSpeed;

    [Header("Jump")]
    public float jumpForce;

    [Header("Raycast - Ground")]
    public LayerMask groundMask;
    public float rayLenght;
    #endregion

    #region Private_Variables
    Vector2 horizontalMovement;
    Vector3 slowdown;

    bool isGrounded;
    bool jumpPressed;
    Ray ray;
    RaycastHit hit;
    Rigidbody rb;
    float horizontal;
    float vertical;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Start()
    {
        
    }

    
    void Update()
    {
        InputPlayer();
        JumpPressed();
    }
    private void FixedUpdate()
    {
        IsGrounded();
        Movement();
        Jump();
    }

    void InputPlayer()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    void Movement()
    {
        //Inicio: limitamos la velocidad máxima del Rigidbody

        //Nos guardamos la velocidad de movimiento que lleva el rigidbody,
        //para acceder a ella
        horizontalMovement = new Vector2(rb.velocity.x, rb.velocity.z);
        
        //La magnitude de un vector es como de largo es ese vector(Es el modulo del vector)
        //Representra a que velocidad va,
        //(horizontalMovement.magnitude) me esta representando la velocidad del rigidbody,
        //la velocidad que nos hemos guardado.
        
        //Si esa magnitud es mayor a la velocidad maxima
        if (horizontalMovement.magnitude > maxSpeed)
        {
            //Normalizamos el vector para que siga apuntando a la misma dirección
            horizontalMovement = horizontalMovement.normalized;
            //Lo multiplico por la velocidad máxima para que se mueve en esa dirección a
            //max velocidad
            horizontalMovement = horizontalMovement * maxSpeed;
        }
        //Asigno velocidad al rigidbody
        //horizontalMovement.x(velocidad del eje x) rb.velocity.y
        //(velocidad en el eje y, que no la hemos tocado. Esto se hará con el jump) y
        //horizontalMovement.y(velocidad en el eje z) que como horizontal.movement
        //es un vector 2 tiene un x y un y, entonces el z lo guardamos en y
        rb.velocity = new Vector3(horizontalMovement.x, rb.velocity.y, horizontalMovement.y);

        //Si está en el suelo le añado su fuerza de para hacer el movimiento
        if (isGrounded)
            //AddRelativeForce es una fuerza que añade a los ejes locales
            //(Van a ser los propios de la capsula no los de la escena)
            //Mi input horizontal * la velocidad de aceleración * Time.deltatime, eje Y 0,
            //Mi input vertical * la velocidad de aceleración * Time.deltatime
            rb.AddRelativeForce(horizontal * accelerationSpeed * Time.deltaTime, 0,
                vertical * accelerationSpeed * Time.deltaTime);
        
        //Si salto y estoy en el aire no me moy a desplazar igual de rapido,
        //le aplicamos la mitad de la aceleración
        else
            rb.AddRelativeForce(horizontal * accelerationSpeed / 2 * Time.deltaTime, 0,
                vertical * accelerationSpeed / 2 * Time.deltaTime);

        //Desaceleramos el rigidbody
        if (isGrounded)
            //El SmoothDamp cambia radualmente un Vector hacia un valor deseado
            //a lo largo del tiempo
            //Coge la velocidad que del rigidbody y llevala a 0, la velocidad de la
            //desaceleración va a ser muy pequeña
            rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(0, rb.velocity.y, 0),
                ref slowdown, desaccelerationSpeed);
    }
    void IsGrounded()
    {
        //El punto de origen del raycast va a estar en el punto pivote del player que está en el medio,
        //al estar a una altura de 1 en este caso le pondremos una medida de 1.1
        //para que toque el suelo
        ray.origin = transform.position;
        //Dirección hacia el ground
        ray.direction = -transform.up;

        //Lanzamos el raycast, la variable out hit para saber si está chocando,
        //la longitud del rayo y un groundMask ya que hacemos un raycast selectivo que
        //solo va a detectar los objetos que están en la capa suelo
        if (Physics.Raycast(ray, out hit, rayLenght, groundMask))
            //Si cumple la condición, tocamos suelo. Si choca con algo, va a ser que esta
            //chocando con en el suelo porque es un raycast selectivo y tiene la longitud
            //que le hemos puesto
            isGrounded = true;
        else
            isGrounded = false;
        //Debugueamos rayo para ver que está haciendo
        Debug.DrawRay(ray.origin, ray.direction * rayLenght, Color.red);
    }
    
    #region Jump

    void JumpPressed()
    {
        //Si el jugagor/a pulsa la tecla espacio y está sobre el suelo, puede saltar
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) jumpPressed = true;
    }
    void Jump()
    {
        //Si es true
        if (jumpPressed)
        {
            //Lo ponemos a falso, para que en el siguiente frame no se vuelva
            //a meter en el if
            jumpPressed = false;
            //Le añadimos una fuerza relativa hacia arriba
            rb.AddRelativeForce(Vector3.up * jumpForce);
        }
    }
    #endregion
}
