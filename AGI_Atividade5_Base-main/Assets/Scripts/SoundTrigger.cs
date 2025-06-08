using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public GameObject soundObject;
    bool Interact;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact = true;
        }
        else
        {
            Interact= false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Interact)
        {
            soundObject.SetActive(true);
        }
    }
}
