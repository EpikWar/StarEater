using UIScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("Box Dimension")]
    [SerializeField] private Vector3 boxSize = new(15, 7, 15);
    [SerializeField] private Quaternion boxRotation;


    private void FixedUpdate()
    {
        if (Physics.CheckBox(transform.position, boxSize, boxRotation, 
                LayerMask.GetMask("Player"), QueryTriggerInteraction.Ignore))
        {
            GameUserInterface.instance.EnterHangar(true);
            if (Input.GetKey("f"))
                SceneManager.LoadScene("Fit Hangar", LoadSceneMode.Single);
        }
        else
        {
            GameUserInterface.instance.EnterHangar(false);
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxSize * 2);
    }

    private void OnTriggerEnter(Collider collisionCollider)
    {
        if (collisionCollider.GetComponentInParent<PlayerManager>())
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
