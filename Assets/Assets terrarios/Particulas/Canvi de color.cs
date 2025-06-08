using UnityEngine;

public class ChispasColorChanger : MonoBehaviour
{
    [Header("Material")]
    public Material material;

    [Header("Colores")]
    public Color[] baseColors;
    public Color[] emissionColors;

    [Header("Emisión")]
    [Range(0f, 10f)] public float emissionIntensity = 2f;

    [Header("Cambios")]
    public float tiempoEntreCambios = 0.1f;

    private float timer = 0f;
    //private ParticleSystem particles;
    //public Transform toFollow;
    //private Transform initialPos;

    void Start()
    {
        if (material == null)
        {
            Debug.LogError("No se ha asignado un material.");
        }

        //particles = GetComponent<ParticleSystem>();

        //initialPos = toFollow.transform;
    }

    void Update()
    {
        if (material == null/* || particles == null || toFollow == null*/) return;

        timer += Time.deltaTime;
        if (timer >= tiempoEntreCambios)
        {
            CambiarColor();
            timer = 0f;
        }

        //var shape = particles.shape;

        //shape.position = transform.InverseTransformPoint(toFollow.position);
    }

    void CambiarColor()
    {
        int index = Random.Range(0, Mathf.Min(baseColors.Length, emissionColors.Length));

        Color baseColor = baseColors[index];
        Color emissionColor = emissionColors[index] * emissionIntensity;

        material.SetColor("_BaseColor", baseColor);
        material.SetColor("_EmissionColor", emissionColor);
        material.EnableKeyword("_EMISSION");
    }
}
