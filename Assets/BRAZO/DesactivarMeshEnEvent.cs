using UnityEngine;

public class DesactivarGameObjectEnEvent : MonoBehaviour
{
    [Tooltip("El GameObject que es desactivarà")]
    public GameObject objecteADesactivar;

    // Aquesta funció s’ha de cridar des d’un Animation Event
    public void DesactivarObjecte()
    {
        if (objecteADesactivar != null)
        {
            objecteADesactivar.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No s’ha assignat cap GameObject a 'objecteADesactivar'");
        }
    }
}
