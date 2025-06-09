using System.Data;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public GameObject soundObject;
    bool Interact;
    bool Interactable;

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

        if (Interactable && Interact)
        {
            soundObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interactable = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Interactable = false;
        }
    }
}
