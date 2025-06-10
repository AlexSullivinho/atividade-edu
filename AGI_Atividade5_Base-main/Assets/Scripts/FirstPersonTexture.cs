using UnityEngine;

public class FirstPersonTexture : MonoBehaviour
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
