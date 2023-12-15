using UIScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    
    
    private void Start()
    {
        
    }

    private void OnTriggerStay(Collider collisionCollider)
    {
        if (collisionCollider == PlayerManager.instance.GetComponentInChildren<Collider>())
        {
            GameUserInterface.instance.EnterHangar(true);
            if (Input.GetKey("f"))
                SceneManager.LoadScene("Fit Hangar", LoadSceneMode.Single);
        }
        
        
    }

    private void OnTriggerExit(Collider collisionCollider)
    {
        if (collisionCollider == PlayerManager.instance.GetComponentInChildren<Collider>())
            GameUserInterface.instance.EnterHangar(false);
        
    }
}
