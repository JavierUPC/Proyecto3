using UnityEngine;

public class ChispasColorChanger : MonoBehaviour
{
    [Header("Material")]
    public Material material;  // El material que usa el Particle System (asignado en el inspector)

    [Header("Colores")]
    public Color[] baseColors;
    public Color[] emissionColors;

    [Header("Emisión")]
    [Range(0f, 10f)] public float emissionIntensity = 2f;

    [Header("Cambios")]
    public float tiempoEntreCambios = 0.1f;

    private float timer = 0f;

    void Start()
    {
        if (material == null)
        {
            Debug.LogError("No se ha asignado un material.");
        }
    }

    void Update()
    {
        if (material == null) return;

        timer += Time.deltaTime;
        if (timer >= tiempoEntreCambios)
        {
            CambiarColor();
            timer = 0f;
        }
    }

    void CambiarColor()
    {
        int index = Random.Range(0, Mathf.Min(baseColors.Length, emissionColors.Length));

        Color baseColor = baseColors[index];
        Color emissionColor = emissionColors[index] * emissionIntensity;

        material.SetColor("_BaseColor", baseColor);
        material.SetColor("_EmissionColor", emissionColor);
        material.EnableKeyword("_EMISSION"); // Necesario para que el color emisivo funcione
    }
}
