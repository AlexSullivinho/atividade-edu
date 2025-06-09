using UnityEngine;

public class PersonTexture : MonoBehaviour
{
    [SerializeField] GameObject Player;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Player.SetActive(!Player.activeSelf);
        }
    }
}
