using TMPro;
using UnityEngine;

public class WinConditions : MonoBehaviour
{
    public GameObject[] collectibles;
    public GameObject endGameOptions;
    public TextMeshProUGUI collectibleQuantityText;

    private int totalCollectibles;
    private int collectedCount = 0;

    private void Start()
    {
        totalCollectibles = collectibles.Length;
        UpdateCollectibleQuantity();
    }

    private void UpdateCollectibleQuantity()
    {
        collectibleQuantityText.text = $"Coletou: {collectedCount} de {totalCollectibles} Items";
    }

    public void CollectItem()
    {
        collectedCount++;

        UpdateCollectibleQuantity();
    }

    public void TriggerEndGame()
    {
        Time.timeScale = 0f;
        endGameOptions.SetActive(true);
    }
}