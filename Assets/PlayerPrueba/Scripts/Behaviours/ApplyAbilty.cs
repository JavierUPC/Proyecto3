using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyAbilty : MonoBehaviour
{
    private bool abilty = false;
    public Habilidad habilidad;
    private Habilidad activeAbilty;
    private float timer;
    public TipoMosca type;
    public float tiempoHabilidad;
    public GameObject pariclesAbility;

    private void Start()
    {
        pariclesAbility.SetActive(false);
    }

    public void Abilty(TipoMosca tipoMosca)
    {
        type = tipoMosca;
        activeAbilty = habilidad;
        abilty = true;
        activeAbilty.AssignType(tipoMosca);
        pariclesAbility.SetActive(true);
        Debug.Log("Tipo: " + tipoMosca);
    }

    // Update is called once per frame
    void Update()
    {
        if (abilty)
            timer += Time.unscaledDeltaTime;

        if (abilty && timer >= tiempoHabilidad)
        {
            abilty = false;
            timer = 0;
            activeAbilty.Stop();
            activeAbilty = null;
            pariclesAbility.SetActive(false);
            Debug.Log("Abilty Limit Reached");
        }
    }

    public bool BugInMouth()
    {
        return !abilty;
    }
}
