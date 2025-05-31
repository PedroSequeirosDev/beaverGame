using UnityEditor.Callbacks;
using UnityEngine;

public class lumberJack : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    private Transform closestTree;
    private float closestDistance = Mathf.Infinity;

    private Rigidbody2D rb;

    void Start()
    {
      Rigidbody2D rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        FindClosestTree();

        if (closestTree != null)
        {
            Vector2 direction = (closestTree.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, LayerMask.GetMask("Houses"));
            if (hit.collider != null)
            {

                Vector2 avoidDir = Vector2.Perpendicular(direction).normalized;
                direction += avoidDir * 0.5f;
                direction.Normalize();
            }

            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }
    }

    private void FindClosestTree()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        foreach (GameObject tree in trees)
        {
            float distance = Vector3.Distance(transform.position, tree.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTree = tree.transform;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb.linearVelocity = Vector2.zero;
    }
}
