using UnityEngine;
using TMPro;
using UnityEngine.AI;
using KinematicCharacterController.Examples;

public class NPCStatesController : MonoBehaviour
{
    [Header("Estados dos Npcs")]
    public NPCStates currentState = NPCStates.Walking;
    public TextMeshProUGUI StateText;
    public NavMeshAgent agent;

    [Header("Pontos de patrulha")]
    public Transform[] patrolPoints;
    private int currentPatrolPoint;
    private Transform lastPatrolPointBeforeSound;

    [Header("Investigar barulho")]
    private Transform[] currentSearchPoints;
    private int currentSearchIndex;
    private Vector3 heardPosition;

    [Header("Perceguir Jogador")]
    private Transform Player;
    public Transform SearchPoint1;
    public Transform SearchPoint2;
    public Transform SearchPoint3;
    public bool CanSee;

    [Header("Atacar Jogador")]
    float wait;


    private float searchTime = 5f;
    private float elapsedSearchTime = 0f;

    private void Start()
    {
        Player = FindFirstObjectByType<ExampleCharacterController>().transform;
        UpdateState(NPCStates.Walking);
        NextPatrolPoint();
    }

    void FixedUpdate()
    {
        if (wait > 0)
        wait -= Time.deltaTime;
        Debug.Log(Vector3.Distance(transform.position, Player.position));
        switch (currentState)
        {
            case NPCStates.Walking:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    NextPatrolPoint();
                break;

            case NPCStates.HeardNoise:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    if (currentSearchPoints != null && currentSearchPoints.Length > 0)
                    {
                        currentSearchIndex = 0;
                        agent.SetDestination(currentSearchPoints[currentSearchIndex].position);
                        UpdateState(NPCStates.Searching);
                    }
                    else
                    {
                        UpdateState(NPCStates.GaveUpSearch);
                    }
                }
                break;

            case NPCStates.Searching:
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    currentSearchIndex++;
                    if (currentSearchIndex < currentSearchPoints.Length)
                    {
                        agent.SetDestination(currentSearchPoints[currentSearchIndex].position);
                        elapsedSearchTime = 0f;
                    }
                    else
                    {
                        UpdateState(NPCStates.GaveUpSearch);
                    }
                }

                elapsedSearchTime += Time.deltaTime;
                if (elapsedSearchTime >= searchTime)
                {
                    UpdateState(NPCStates.GaveUpSearch);
                }
                break;
            case NPCStates.Chasing:
                agent.SetDestination(Player.position);
                if (!CanSee)
                {
                    SearchPoint1.position = Player.position + Player.transform.forward * 10;
                    SearchPoint2.position = Player.position + Player.transform.forward * 10 + Player.transform.right * 5;
                    SearchPoint3.position = Player.position + Player.transform.forward * 10 - Player.transform.right * 5;
                    Transform[] Lookers;
                    Lookers = new Transform[3];
                    Lookers[0] = SearchPoint1;
                    Lookers[1] = SearchPoint2;
                    Lookers[2] = SearchPoint3;
                    UpdateState(NPCStates.Walking);
                    HeardSound(Player.position, Lookers);
                }
                if (Vector3.Distance(transform.position, Player.position) <= 2)
                {
                    UpdateState(NPCStates.Attacking);
                }
                    break;
            case NPCStates.Attacking:
                if (Vector3.Distance(transform.position, Player.position) < 3 && wait <= 0)
                {
                    Debug.Log("Atack");
                    WinConditions End = GameManager.FindFirstObjectByType<WinConditions>();
                    End.TriggerEndGame();
                }
                else if (wait <= 0)
                {
                    UpdateState(NPCStates.Chasing);
                }
                break;
        }
    }

    public void UpdateState(NPCStates newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case NPCStates.Walking:
                StateText.text = "NPC Andando";
                agent.speed = 2f;
                break;

            case NPCStates.Running:
                StateText.text = "NPC correndo";
                agent.speed = 5f;
                break;

            case NPCStates.HeardNoise:
                StateText.text = "NPC ouviu um barulho";
                agent.speed = 5f;
                lastPatrolPointBeforeSound = patrolPoints[currentPatrolPoint];
                agent.SetDestination(heardPosition);
                break;

            case NPCStates.Searching:
                StateText.text = "NPC procurando o jogador";
                elapsedSearchTime = 0f;
                if (CanSee)
                {
                    UpdateState(NPCStates.Chasing);
                }
                break;

            case NPCStates.GaveUpSearch:
                StateText.text = "NPC desistiu de procurar o jogador";
                currentSearchPoints = null;
                agent.SetDestination(lastPatrolPointBeforeSound.position);
                UpdateState(NPCStates.Walking);
                break;

            case NPCStates.Chasing:
                StateText.text = "NPC te encontrou!";
                agent.speed = 6f;
                break;

            case NPCStates.Attacking:
                StateText.text = "NPC atacando!";
                agent.SetDestination(transform.position);
                agent.speed = 0;
                if (wait <= 0)
                wait = .5f;
                break;
        }
    }

    public void NextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPatrolPoint].position);
        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
    }

    public void HeardSound(Vector3 position, Transform[] searchPoints)
    {
        if (currentState == NPCStates.Walking || currentState == NPCStates.GaveUpSearch)
        {
            heardPosition = position;
            currentSearchPoints = searchPoints;
            UpdateState(NPCStates.HeardNoise);
        }
    }
}
