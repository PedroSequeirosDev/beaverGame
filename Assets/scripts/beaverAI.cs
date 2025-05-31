using UnityEngine;

public class beaverAI : MonoBehaviour
{
    public BeaverProfession profession = BeaverProfession.Idle;
    public GameObject targetObject;
    public float stopDistance = 0.5f;
    private beaverMovement movement;
    private bool hasArrived = false;

    void Start()
    {
        movement = GetComponent<beaverMovement>();
        // Auto-assign targets if not set
        if (profession == BeaverProfession.Lumberjack && targetObject == null)
        {
            var trees = GameObject.FindGameObjectsWithTag("Tree");
            if (trees.Length > 0)
                targetObject = trees[Random.Range(0, trees.Length)];
        }
        else if (profession == BeaverProfession.DamWorker && targetObject == null)
        {
            var dams = GameObject.FindGameObjectsWithTag("Dam");
            if (dams.Length > 0)
                targetObject = dams[Random.Range(0, dams.Length)];
        }
    }

    void Update()
    {
        if (profession == BeaverProfession.Idle)
        {
            // Stay near a house
            if (targetObject != null)
                MoveToTarget(targetObject.transform.position);
        }
        else if (profession == BeaverProfession.Lumberjack || profession == BeaverProfession.DamWorker)
        {
            if (targetObject != null)
                MoveToTarget(targetObject.transform.position);
        }
        else if (profession == BeaverProfession.Builder)
        {
            if (targetObject != null)
            {
                MoveToTarget(targetObject.transform.position);
                // Try to build if close enough
                if (Vector2.Distance(transform.position, targetObject.transform.position) <= stopDistance)
                {
                    var house = targetObject.GetComponent<beaverHouse>();
                    if (house != null && !house.IsBuilt)
                        house.Build(0.5f * Time.deltaTime); // 0.5 per second per beaver
                }
            }
        }
    }

    float GetEdgeToEdgeDistance(GameObject target, float extraGap = 0f)
    {
        float myRadius = 0f;
        var myCircle = GetComponent<CircleCollider2D>();
        if (myCircle != null)
            myRadius = myCircle.radius * Mathf.Abs(transform.localScale.x);

        float targetRadius = 0f;
        if (target != null)
        {
            var targetCircle = target.GetComponent<CircleCollider2D>();
            if (targetCircle != null)
                targetRadius = targetCircle.radius * Mathf.Abs(target.transform.localScale.x);
        }
        return myRadius + targetRadius + extraGap;
    }

    void MoveToTarget(Vector2 targetPos)
    {
        if (targetObject == null) return;
        float stopDist = GetEdgeToEdgeDistance(targetObject, 0.1f); // 0.1f is extra gap
        Vector2 dir = (targetPos - (Vector2)transform.position);

        if (dir.magnitude > stopDist)
        {
            movement.SetMoveDirection(dir.normalized);
            hasArrived = false;
        }
        else
        {
            if (!hasArrived)
            {
                movement.SetMoveDirection(Vector2.zero);
                hasArrived = true;
            }
        }
    }
}