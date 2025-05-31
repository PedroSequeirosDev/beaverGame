using UnityEditor.Callbacks;
using UnityEngine;

public class lumberJack : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    private Transform closestTree;
    private float closestDistance = Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        FindClosestTree();

        if (closestTree != null)
        {
            Vector3 direction = (closestTree.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(closestTree);
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

    private void 
}
