using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private int life = 100;
    [SerializeField] private Slider barraVida;


    private void Start()
    {
        barraVida.maxValue = life;
        barraVida.value = life;
    }

    private void Update()
    {
        if (life == 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            barraVida.value = life;
        }
    }

    public void RestarVida() {
        this.life--;
    }

    public void SumarVida()
    {
        if (this.life <= 90)
        {
            this.life += 10;
        }
        else if (this.life > 90 && this.life < 100) {
            this.life = 100;
        }
    }


}
