using KinematicCharacterController.Examples;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCFov : MonoBehaviour
{
    Transform Player;
    float Angle;
    public float FOVDistance;
    public LayerMask Wall;
    public LayerMask PlayerLayer;
    NPCStatesController Controller;

    private void Awake()
    {
        Player = FindFirstObjectByType<ExampleCharacterController>().transform;
        Controller = GetComponent<NPCStatesController>();
    }

    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, Player.position, Color.red);
        Controller.CanSee = ChaserView();
        if (Finders() && Controller.currentState != NPCStates.Attacking)
        {
            Controller.UpdateState(NPCStates.Chasing);
        }
    }

    private bool Finders()
    {
        Vector3 target = Player.position - transform.position;
        Angle = Vector3.Angle(target, transform.forward);
        if (!Physics.Raycast(transform.position, target, FOVDistance, Wall))
        {
            if (Physics.Raycast(transform.position, target, FOVDistance, 3) && Angle <= 70)
            {
                return true;
            }
            if (Physics.CheckSphere(transform.position, 3, PlayerLayer))
            {
                return true;
            }
        }
        return false;

    }
    private bool ChaserView()
    {
        return (Physics.Raycast(transform.position, Player.position - transform.position, 20, PlayerLayer) && Angle <= 80);
    }
}
