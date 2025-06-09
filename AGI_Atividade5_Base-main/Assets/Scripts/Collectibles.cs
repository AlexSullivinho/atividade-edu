using UnityEngine;

public class Collectibles : MonoBehaviour
{
    public WinConditions winConditions;

    private bool isCollected = false;
    private float Initial_Ypos;
    private float Ypos;
    bool UpnDown;

    private void Start()
    {
        Initial_Ypos = transform.position.y;
    }

    private void FixedUpdate()
    {
        Ypos = transform.position.y;
        transform.eulerAngles += new Vector3(0, 100f, 0) * Time.deltaTime;
        if (Ypos > Initial_Ypos + .25f)
        {
            UpnDown = true;
        }
        if (Ypos < Initial_Ypos - .25f)
        {
            UpnDown = false;
        }
        if (UpnDown)
        {
            transform.position -= new Vector3(0, .5f, 0) * Time.deltaTime;
        }
        if (!UpnDown)
        {
            transform.position += new Vector3(0, .5f, 0) * Time.deltaTime;
        }
    }

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