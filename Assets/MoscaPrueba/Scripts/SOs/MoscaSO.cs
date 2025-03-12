using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoMosca
{
    None,
    Electrico,
    Fuego
}

[CreateAssetMenu(fileName = "NuevoTipoMosca", menuName = "Nuevo Tipo Mosca")]
public class MoscaSO : ScriptableObject
{
    public TipoMosca mosca;
}
