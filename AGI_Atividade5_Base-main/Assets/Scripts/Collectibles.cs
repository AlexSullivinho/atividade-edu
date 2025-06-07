using UnityEngine;

public class Collectibles : MonoBehaviour
{
    public WinConditions winConditions;

    private bool isCollected = false;

    public void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            isCollected = true;

            winConditions.CollectItem();

            gameObject.SetActive(false);
        }
    }
}