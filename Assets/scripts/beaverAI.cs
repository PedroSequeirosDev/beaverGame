using UnityEngine;

public class beaverAI : MonoBehaviour
{
    public BeaverProfession profession = BeaverProfession.Idle;
    public GameObject targetObject;
    public float stopDistance = 0.5f;
    private beaverMovement movement;
    private bool hasArrived = false;
    private float woodTimer = 0f;
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
        else if (profession == BeaverProfession.Lumberjack)
        {
            if (targetObject != null)
            {
                MoveToTarget(targetObject.transform.position);
                if (IsTouchingTarget())
                {
                    woodTimer += Time.deltaTime;
                    if (woodTimer >= 2f)
                    {
                        woodTimer = 0f;
                        // Add wood to inventory
                        if (uiManager.Instance != null)
                            uiManager.Instance.woodInventory += 1;
                    }
                }
                else
                {
                    woodTimer = 0f; // Reset timer if not at target
                }
            }
        }
        else if (profession == BeaverProfession.DamWorker)
        {
            if (targetObject != null)
            {
                MoveToTarget(targetObject.transform.position);
                if (IsTouchingTarget())
                {
                    var dam = targetObject.GetComponent<beaverHouse>(); // Or beaverDam if you have a separate script
                    if (dam != null && !dam.IsBuilt)
                    {
                        if (uiManager.Instance != null && uiManager.Instance.woodInventory >= 5 * Time.deltaTime)
                        {
                            dam.Build(0.5f * Time.deltaTime);
                            uiManager.Instance.woodInventory -= (int)(5 * Time.deltaTime);
                        }
                    }
                    else
                    {
                        // Try to find a new unbuilt dam
                        AssignTargetForProfession();
                    }
                }
            }
            else
            {
                // Try to find a new unbuilt dam if none assigned
                AssignTargetForProfession();
            }
        }
        else if (profession == BeaverProfession.Builder)
        {
            if (targetObject != null)
            {
                MoveToTarget(targetObject.transform.position);
                if (IsTouchingTarget())
                {
                    var house = targetObject.GetComponent<beaverHouse>();
                    if (house != null && !house.IsBuilt)
                    {
                        house.Build(0.5f * Time.deltaTime);
                    }
                    else
                    {
                        // Only try to find a new unbuilt house if the current one is built or missing
                        var houses = GameObject.FindGameObjectsWithTag("House");
                        GameObject newTarget = null;
                        foreach (var h in houses)
                        {
                            var houseScript = h.GetComponent<beaverHouse>();
                            if (houseScript != null && !houseScript.IsBuilt)
                            {
                                newTarget = h;
                                break;
                            }
                        }
                        if (newTarget != null)
                            targetObject = newTarget;
                        // If no new unbuilt house, stay at the last target (do nothing)
                    }
                }
            }
            else
            {
                // If we have no target, try to find an unbuilt house
                var houses = GameObject.FindGameObjectsWithTag("House");
                foreach (var h in houses)
                {
                    var houseScript = h.GetComponent<beaverHouse>();
                    if (houseScript != null && !houseScript.IsBuilt)
                    {
                        targetObject = h;
                        break;
                    }
                }
                // If still none, just wait (do nothing)
            }
        }
    }

    public void AssignTargetForProfession()
    {
        if (profession == BeaverProfession.Lumberjack)
        {
            var trees = GameObject.FindGameObjectsWithTag("Tree");
            if (trees.Length > 0)
            {
                GameObject closest = null;
                float closestDist = float.MaxValue;
                Vector3 myPos = transform.position;
                foreach (var tree in trees)
                {
                    float dist = (tree.transform.position - myPos).sqrMagnitude;
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = tree;
                    }
                }
                targetObject = closest;
            }
            else
            {
                targetObject = null;
            }
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
            var houses = GameObject.FindGameObjectsWithTag("House");
            if (houses.Length > 0)
            {
                // Pick the closest house
                GameObject closest = null;
                float closestDist = float.MaxValue;
                Vector3 myPos = transform.position;
                foreach (var house in houses)
                {
                    float dist = (house.transform.position - myPos).sqrMagnitude;
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = house;
                    }
                }
                targetObject = closest;
            }
            else
            {
                targetObject = null;
            }
        }
        else if (profession == BeaverProfession.Builder)
        {
            var houses = GameObject.FindGameObjectsWithTag("House");
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