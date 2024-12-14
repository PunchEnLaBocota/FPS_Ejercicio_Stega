using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    #region Public_variables
    public float mouseSensitivity;
    public float bottomAngles;//Limite giro de la camára en el eje X
    public float topAngle;
    public float yRotationSpeed;
    public float xCameraSpeed;
    #endregion

    #region
    float desiredYRotation;//La rotación deseada para la capsula en el eje Y
    float desiredCameraXRotation;
    float currentYRotation;
    float currentCameraXRotation;
    float rotationYVelocity;
    float cameraXVelocity;

    Camera myCamera;
    float mouseX;
    float mouseY;
    #endregion

    private void Awake()
    {

        myCamera = Camera.main;//Hago referencia a la cámara princpial
        Cursor.lockState = CursorLockMode.Locked;//Bloqueamos el cursor
    }

    void Start()
    {
        
    }

   
    void Update()
    {
        MouseInputMovement();
    }
    private void FixedUpdate()
    {
        //Lo metemos el Fixed para que el giro vaya más estable al aplicarlo
        ApplyRotation();
    }
    void MouseInputMovement()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        //El giro que quiero para la capsula(player) en torno al ejeY, lo guardamos
        //en la variable desiredYRotation
        desiredYRotation = desiredYRotation + (mouseX * mouseSensitivity);
        //El giro que quiero para la cámara en torno al ejeX, lo guardamos
        //en la variable desiredCameraXRotation
        desiredCameraXRotation = desiredCameraXRotation - (mouseY * mouseSensitivity);
       //limitamos el valor del ángulo de la cámara con un mínimo y un máximo
        desiredCameraXRotation = Mathf.Clamp(desiredCameraXRotation, bottomAngles, topAngle);
    }

    void ApplyRotation()
    {
        //Tenemos una rotación actual(currentYRotation) una rotación deseada
        //(desiredYRotation) y queremos que haga ese cambio de rotación actual a
        // la rotación deseada  con una velocidad
        currentYRotation = Mathf.SmoothDamp(currentYRotation, desiredYRotation,
            ref rotationYVelocity, yRotationSpeed);
        //Lo mismo con la rotación de la camara
        currentCameraXRotation = Mathf.SmoothDamp(currentCameraXRotation, 
            desiredCameraXRotation, ref cameraXVelocity, xCameraSpeed);

        //Giro la cápsula
        //currentYRotation es el ángulo en grados que quiero aplicarle a la cápsula
        //en su eje Y
        //transform.rotation es un quaternion
        //Con Quaternion.Euler puedo girar el objeto x grados a lo largo del eje que quiera
        //(o los 3 ejes)
        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);

        //Giro la cámara
        myCamera.transform.localRotation = Quaternion.Euler(currentCameraXRotation, 0, 0);
    }

}
