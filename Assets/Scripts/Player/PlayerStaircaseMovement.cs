using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStaircaseMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;

    [SerializeField] private float speed = 4f;

    [SerializeField] private GameObject staircase;
    [SerializeField] private List<Transform> waypoints;
    private int currentWaypointIndex = 0;

    private float bottomStairEndX;
    private float topStairEndX;
    public float stairMovementDirY = 0;
    private bool atStairBottom = false;
    public bool atStairTop = false;
    public bool movingOnStairs = false;
    public bool atFinalWaypoint = false;
    public enum StairDirection { left, right, up }
    public StairDirection stairDirection = StairDirection.right;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if (waypoints.Count == 0)
        {
            if (playerMovement.dirY > 0 && atStairBottom && playerMovement.IsGrounded())
            {
                if (bottomStairEndX - .1f > transform.position.x)
                {
                    playerMovement.dirX = 1;
                }
                else if (bottomStairEndX + .1f < transform.position.x)
                {
                    playerMovement.dirX = -1;
                }
                else
                {
                    MountStaircase();
                    playerMovement.dirX = 0;
                }
            }
            else if (playerMovement.dirY < 0 && atStairTop && !atStairBottom && playerMovement.IsGrounded())
            {
                if (topStairEndX - .1f > transform.position.x)
                {
                    playerMovement.dirX = 1;
                }
                else if (topStairEndX + .1f < transform.position.x)
                {
                    playerMovement.dirX = -1;
                }
                else
                {
                    MountStaircase();
                    playerMovement.dirX = 0;
                }
            }
        }
        else
        {
            GetStairMovementDirY();

            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
            {
                playerMovement.canTurn = true;

                if (stairMovementDirY > 0)
                {
                    currentWaypointIndex++;
                }
                else if (stairMovementDirY < 0)
                {
                    currentWaypointIndex--;
                }
                else
                {
                    movingOnStairs = false;
                }

                if (currentWaypointIndex >= waypoints.Count - 1)
                {
                    atFinalWaypoint = true;
                }
                else
                {
                    atFinalWaypoint = false;
                }

                if (currentWaypointIndex >= waypoints.Count || currentWaypointIndex < 0)
                {
                    UnMountStaircase();
                    return;
                }
                if (stairDirection == StairDirection.up && atFinalWaypoint && playerMovement.dirX != 0)
                {
                    UnMountStaircase();
                    return;
                }
            }
            else
            {
                playerMovement.canTurn = false;
                movingOnStairs = true;
            }

            if (!playerAttack.inAttackAnimation)
            {
                transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
            }
        }
    }

    private void MountStaircase()
    {
        foreach (Transform child in staircase.transform)
        {
            waypoints.Add(child);
        }

        if (atStairBottom)
        {
            currentWaypointIndex = 0;
        }
        else if(atStairTop)
        {
            currentWaypointIndex = waypoints.Count - 1;
        }

        GetStairMovementDirY();

        playerMovement.onStairs = true;
        coll.isTrigger = true;
        rb.gravityScale = 0f;
    }

    public void UnMountStaircase()
    {
        waypoints.Clear();

        movingOnStairs = false;
        playerMovement.onStairs = false;
        playerMovement.canTurn = true;
        coll.isTrigger = false;
        rb.gravityScale = 3.5f;
        playerMovement.dirY = 0;
    }

    private void GetStairMovementDirY()
    {
        if (stairDirection == StairDirection.right)
        {
            stairMovementDirY = Mathf.Clamp(playerMovement.dirY + playerMovement.dirX, -1, 1);
        }
        else if (stairDirection == StairDirection.left)
        {
            stairMovementDirY = Mathf.Clamp(playerMovement.dirY + -playerMovement.dirX, -1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "StairBottom")
        {
            staircase = collision.transform.parent.gameObject;

            atStairBottom = true;

            bottomStairEndX = collision.transform.position.x;

            if (collision.name == "LeftEnd")
            {
                stairDirection = StairDirection.right;
            }
            else if (collision.name == "RightEnd")
            {
                stairDirection = StairDirection.left;
            }
            else
            {
                stairDirection = StairDirection.up;
            }
        }
        else if (collision.tag == "StairTop")
        {
            staircase = collision.transform.parent.gameObject;

            atStairTop = true;

            topStairEndX = collision.transform.position.x;

            if (collision.name == "LeftEnd")
            {
                stairDirection = StairDirection.left;
            }
            else if (collision.name == "RightEnd")
            {
                stairDirection = StairDirection.right;
            }
            else
            {
                stairDirection = StairDirection.up;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "StairBottom")
        {
            atStairBottom = false;
        }
        if (collision.tag == "StairTop")
        {
            atStairTop = false;
        }
    }
}
