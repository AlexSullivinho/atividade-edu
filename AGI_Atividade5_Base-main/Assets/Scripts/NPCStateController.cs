using UnityEngine;
using TMPro;
using UnityEngine.AI;

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

    private float searchTime = 5f;
    private float elapsedSearchTime = 0f;

    private void Start()
    {
        UpdateState(NPCStates.Walking);
        NextPatrolPoint();
    }

    void FixedUpdate()
    {
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
                break;

            case NPCStates.GaveUpSearch:
                StateText.text = "NPC desistiu de procurar o jogador";
                currentSearchPoints = null;
                agent.SetDestination(lastPatrolPointBeforeSound.position);
                UpdateState(NPCStates.Walking);
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
