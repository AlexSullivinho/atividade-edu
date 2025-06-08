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

    private void OnEnable()
    {
        Emit();
    }

    public void Emit()
    {
        audioSource.PlayOneShot(audioSource.clip);

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
