using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Butterfly : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Animator animator = null;
    private int animatorIsFlying;
    public List<Transform> movingPoints = new List<Transform>();
    private float t;
    public int currentPointIndex = 0;

    public int nextPointIndex = 0;
    
    void Start()
    {
        animatorIsFlying = Animator.StringToHash("isFlying");
        transform.position = movingPoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Keyboard.current.spaceKey.isPressed)
        // {
        //     UpdatePosition();
        // }
        MoveToNextPoint();
    }

    private void MoveToNextPoint()
    {
        if (currentPointIndex == nextPointIndex)
        {
            return;
        }
        t += Time.deltaTime/speed;
        transform.position = Vector3.Lerp(movingPoints[currentPointIndex].position, movingPoints[nextPointIndex].position, Mathf.SmoothStep(0, 1, t));
        
        if (transform.position == movingPoints[nextPointIndex].position)
        {
            currentPointIndex = nextPointIndex;
            t = 0;
            animator.SetBool(animatorIsFlying, false);
        }
    }

    public void UpdatePosition()
    {
        nextPointIndex = currentPointIndex + 1;
        Debug.Log("Update position: " + currentPointIndex + " to " + nextPointIndex);
        animator.SetBool(animatorIsFlying, true);
    }
}
