using UnityEngine;

public class DesactivarGameObjectEnEvent : MonoBehaviour
{
    [Tooltip("El GameObject que es desactivar�")]
    public GameObject objecteADesactivar;

    // Aquesta funci� s�ha de cridar des d�un Animation Event
    public void DesactivarObjecte()
    {
        if (objecteADesactivar != null)
        {
            objecteADesactivar.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No s�ha assignat cap GameObject a 'objecteADesactivar'");
        }
    }
}
