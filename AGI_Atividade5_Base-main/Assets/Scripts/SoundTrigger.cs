using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public GameObject soundObject;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            soundObject.SetActive(true);
        }
    }
}
