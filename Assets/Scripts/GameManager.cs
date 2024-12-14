using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Target targetSelect;
    public Color colorSelect;

    public Image healthImageUI;

    public GameObject arrow;
    public CanvasTarget canvasTarget;

    public static GameManager gameManager;

    private void Awake()
    {
        gameManager = this;
    }

    void Start()
    {
        
    }

  
    void Update()
    {
        
    }

    public void SelectTarget(Target target)
    {
        //Si targetSelect es igual a target, es decir si estoy seleccionando el target
        //que ya tengo marcado
        if(targetSelect == target)
        {
            //Activamos o desactivamos (dependiendo del estado que tenga) la flecha y la 
            //barra de la interfaz correspondiente a la vida del cubo
            arrow.SetActive(!arrow.activeInHierarchy);
            healthImageUI.gameObject.SetActive(!healthImageUI.gameObject.activeInHierarchy);
            //Si la flecha queda desactivada, significa que he deseleccionado el cubo y
            //por lo tanto mi target es nulo
            if (!arrow.activeInHierarchy) targetSelect = null;
        }
        else
        {
            //Activo flecha
            arrow.SetActive(true);
            //Posiciono la flecha
            canvasTarget.target = target.transform;
            //Activo la barrita de vida de la interfaz
            healthImageUI.gameObject.SetActive(true);
            //Asigno color
            healthImageUI.color = target.colorTarget;
            //Asigno vida
            healthImageUI.fillAmount = target.currentHealth / target.maxHealth;
            //Digo este es mi target
            targetSelect = target;

        }
    }
    public void DeathTarget()
    {
        //Desactiva flecha
        arrow.SetActive(false);
        //Desactiva barrita de vida de la interfaz
        healthImageUI.gameObject.SetActive(false);
        //Y el target a null
        targetSelect = null;
    }
    
    //La vamos a llamar desde el target cada vez que el cubo cambie de color
    //va a comprobar si en ese momento es el cubo seleccionado, si es que si,
    //cambiamos el color de la barra de la interfaz
    public void AmITargetSelected(Target target)
    {
        if (target == targetSelect) healthImageUI.color = target.colorTarget;
    }
    
    //Método que va a devolver un true si el target que le pasamos como párametro es igual 
    //al target que tenemos seleccionado, sino devolverá un falso
    public bool CheckTarget(Target target)
    {
        if (target == targetSelect) return true;
        else return false;
    }
}
