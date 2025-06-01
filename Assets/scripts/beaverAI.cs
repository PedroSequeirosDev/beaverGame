using UnityEngine;

public class beaverAI : MonoBehaviour
{
    public BeaverProfession profession = BeaverProfession.Idle;
    public GameObject targetObject;
    public float stopDistance = 0.5f;
    private beaverMovement movement;
    private bool hasArrived = false;
    bool IsTouchingTarget()
    {
        if (targetObject == null) return false;

        var myCollider = GetComponent<Collider2D>();
        var targetCollider = targetObject.GetComponent<Collider2D>();
        if (myCollider == null || targetCollider == null) return false;

        return myCollider.IsTouching(targetCollider);
    }

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

        movement = GetComponent<beaverMovement>();
        AssignTargetForProfession();
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

    public void AssignTargetForProfession()
    {
        if (profession == BeaverProfession.Lumberjack)
        {
            var trees = GameObject.FindGameObjectsWithTag("Tree");
            if (trees.Length > 0)
                targetObject = trees[Random.Range(0, trees.Length)];
            else
                targetObject = null;
        }
        else if (profession == BeaverProfession.DamWorker)
        {
            var dams = GameObject.FindGameObjectsWithTag("Dam");
            if (dams.Length > 0)
                targetObject = dams[Random.Range(0, dams.Length)];
            else
                targetObject = null;
        }
        else if (profession == BeaverProfession.Idle)
        {
            // Optionally, assign a house here if you want
            // Otherwise, beaverManager already assigns a house on spawn
        }
        else if (profession == BeaverProfession.Builder)
        {
            var houses = GameObject.FindGameObjectsWithTag("House");
            // Find an unbuilt house
            foreach (var house in houses)
            {
                var houseScript = house.GetComponent<beaverHouse>();
                if (houseScript != null && !houseScript.IsBuilt)
                {
                    targetObject = house;
                    return;
                }
            }
            targetObject = null;
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

        if (!IsTouchingTarget())
        {
            Vector2 dir = (targetPos - (Vector2)transform.position);
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