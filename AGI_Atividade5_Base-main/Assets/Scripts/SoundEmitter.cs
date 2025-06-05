using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource audioSource;

    public Transform[] searchPoints;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Emit();
    }

    public void Emit()
    {
        audioSource.Play();

        float radius = audioSource.maxDistance;

        Collider[] listeners = Physics.OverlapSphere(transform.position, radius);
        foreach (var col in listeners)
        {
            var npc = col.GetComponent<NPCStatesController>();
            if (npc != null)
            {
                npc.HeardSound(transform.position, searchPoints);
            }
        }
    }
}
