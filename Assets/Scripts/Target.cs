using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public ParticleSystem hitEffect;

    public Color[] colors;//Una array de colores
    public Color colorTarget;//El color que tiene el target actualmente
    public float timeToChangeColor;//El tiempo que va a tardar de color
    
    public Image healthImage;
   
    float timer;//Un timer
    Renderer rend;//Accedemos al material con el componente meshrenderer

    private void Awake()
    {
        rend = GetComponent<Renderer>();    
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

   
    void Update()
    {
        timer += Time.deltaTime;
        ChangeColor();
    }
    void ChangeColor()
    {
        if(timer >= timeToChangeColor)
        {
            //Tenemos un timer
            timer = 0;
            //Una array de colores
            colorTarget = colors[(int)Random.Range(0, colors.Length)];
            //Cambiamos color
            rend.material.color = colorTarget;
            //Avisar al gamemanager de que el cubo ha cambiado de color
            GameManager.gameManager.AmITargetSelected(this);
        }
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthImage.fillAmount = currentHealth/maxHealth;
        if (currentHealth <= 0)
        {
            Death();
        }
    }
    void Death()
    {
        //Llamaos al método DeathTarget del script GameManager
        GameManager.gameManager.DeathTarget();
        Destroy(gameObject);
    }
}
