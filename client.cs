using UnityEngine;
using Pancake;

public class RayInteractorBehaviorScript : MonoBehaviour
{
    private string ObjectName = "Bottle"; 
    private const Pancake.Models.User user = Pancake.GetUser();
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(ObjectName))
        {
            Debug.Log($"{user} touched {ObjectName}");
            Pancake.XAPI.Trigger(user, "touched", ObjectName);
        }
    }
}