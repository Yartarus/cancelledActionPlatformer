using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;

    private PlayerCondition playerCondition;
    private PlayerMovement playerMovement;

    public bool inAttackAnimation = false;

    [SerializeField] private Transform firePoint;
    private Transform fPoint;
    [SerializeField] private GameObject blade;
    [SerializeField] private GameObject[] projectiles;

    [SerializeField] private float throwCooldown;
    private float throwCooldownTimer = Mathf.Infinity;

    [SerializeField] private float animationCanceltime;
    private float animationCancelTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        playerCondition = GetComponent<PlayerCondition>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (inAttackAnimation && animationCancelTimer > animationCanceltime)
        {
            Debug.Log(animationCancelTimer + " > " + animationCanceltime);
            Debug.Log("rEEEEEEEEeEEEEE?");
            EndAttackAnimation();
        }

        throwCooldownTimer += Time.deltaTime;
        animationCancelTimer += Time.deltaTime;
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled || PauseMenu.playerControlsDisabled || inAttackAnimation || playerMovement.hurt || playerMovement.cutsceneMode)
        {
            return;
        }

        anim.SetBool("attack", true);
        inAttackAnimation = true;

        playerMovement.canTurn = true;

        animationCancelTimer = 0;
    }

    public void OnThrow(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled || PauseMenu.playerControlsDisabled || playerCondition.subWeaponEnergy < playerCondition.subWeaponEnergyCost || inAttackAnimation || playerMovement.hurt || playerMovement.cutsceneMode || throwCooldownTimer <= throwCooldown)
        {
            return;
        }

        anim.SetBool("throw", true);
        inAttackAnimation = true;
        throwCooldownTimer = 0;

        playerMovement.canTurn = true;

        animationCancelTimer = 0;
    }

    private void ActivateBlade()
    {
        float direction = playerMovement.CheckSpriteFlip();

        blade.transform.position = firePoint.position;

        if (playerMovement.crouching)
        {
            blade.transform.Translate(0, -.625f, 0);
        }

        if (direction == 1)
        {
            blade.transform.Translate(.5f, 0, 0);
        }
        else
        {
            blade.transform.Translate(-3.5f, 0, 0);
        }

        blade.GetComponent<Blade>().SetDirection(direction, (int)playerCondition.bladeType);
    }

    private void ActivateProjectile()
    {
        playerCondition.UseSubweaponEnergy();

        float direction = playerMovement.CheckSpriteFlip();

        projectiles[FindProjectile()].transform.position = firePoint.position;

        if (playerMovement.crouching)
        {
            projectiles[FindProjectile()].transform.Translate(0, -.625f, 0);
        }

        if (direction != 1)
        {
            projectiles[FindProjectile()].transform.Translate(-3f, 0, 0);
        }

        projectiles[FindProjectile()].GetComponent<Projectile>().SetDirection(direction, (int)playerCondition.subweapon);
    }

    private int FindProjectile()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (!projectiles[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }

    public void EndAttackAnimation()
    {
        blade.SetActive(false);
        anim.SetBool("attack", false);
        anim.SetBool("throw", false);
        inAttackAnimation = false;
        playerMovement.canTurn = true;
    }
}
