using System.Collections;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    private BoxCollider2D coll;
    private SpriteRenderer sprite;

    private PlayerMovement playerMovement;

    [Header ("Health")]
    [SerializeField] private float maxHealth = 17f;
    public float health { get; private set; }

    [Header("IFrames")]
    [SerializeField] private float iFramesDuration;
    private int flashesPerSecond = 30;

    [Header("SubWeapon")]
    [SerializeField] private int maxSubWeaponEnergy = 9;
    [SerializeField] private int initialSubWeaponEnergy = 2;
    public int subWeaponEnergy { get; private set; }
    public int subWeaponEnergyCost { get; private set; }

    public enum SubWeapon { rock, dart, mambele, boomerang, scythe }
    public SubWeapon subweapon { get; private set; }

    [Header("Misc")]
    public bool hasKey;

    public enum BladeType { knife, sword, greatKnife }
    public BladeType bladeType { get; private set; }

    private void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();

        playerMovement = GetComponent<PlayerMovement>();

        health = maxHealth;

        subWeaponEnergy = initialSubWeaponEnergy;
        subweapon = SubWeapon.rock;
        subWeaponEnergyCost = SetSubWeaponEnergyCost();

        hasKey = false;

        bladeType = BladeType.knife;

        if (PlayerPrefs.HasKey("PlayerTakeDefaultValues"))
        {
            LoadCondition();
        }
    }

    public void SaveCondition()
    {
        PlayerPrefs.SetInt("PlayerTakeDefaultValues", ExtensionMethods.BoolToInt(false));

        PlayerPrefs.SetFloat("PlayerHealth", health);

        PlayerPrefs.SetInt("PlayerSubWeaponEnergy", subWeaponEnergy);

        PlayerPrefs.SetInt("PlayerSubweapon", (int)subweapon);

        PlayerPrefs.SetInt("PlayerHasKey", ExtensionMethods.BoolToInt(hasKey));

        PlayerPrefs.SetInt("PlayerBladeType", (int)bladeType);
}

    public void LoadCondition()
    {
        if (!ExtensionMethods.IntToBool(PlayerPrefs.GetInt("PlayerTakeDefaultValues")))
        {
            PlayerPrefs.SetInt("PlayerTakeDefaultValues", ExtensionMethods.BoolToInt(true));

            health = PlayerPrefs.GetFloat("PlayerHealth");

            subWeaponEnergy = PlayerPrefs.GetInt("PlayerSubWeaponEnergy");

            PickupSubweapon((SubWeapon)PlayerPrefs.GetInt("PlayerSubweapon"));

            hasKey = ExtensionMethods.IntToBool(PlayerPrefs.GetInt("PlayerHasKey"));

            PickupWeapon((BladeType)PlayerPrefs.GetInt("PlayerBladeType"));
        }
    }

    public void TakeDamage(float _damage)
    {
        RNG.getRN(0, 1);

        health = Mathf.Clamp(health - _damage, 0, maxHealth);
        StartCoroutine(Invulnerability());

        if (health > 0)
        {
            FindObjectOfType<AudioManager>().Play("PlayerDamage");
        }
        else
        {
            FindObjectOfType<AudioManager>().StopBGM();
        }

        playerMovement.Knockback();
    }

    public void AddHealth(float _value)
    {
        health = Mathf.Clamp(health + _value, 0, maxHealth);
    }

    public void AddSubweaponEnergy(int _value)
    {
        subWeaponEnergy = Mathf.Clamp(subWeaponEnergy + _value, 0, maxSubWeaponEnergy);
    }

    public void UseSubweaponEnergy()
    {
        subWeaponEnergy = Mathf.Clamp(subWeaponEnergy - subWeaponEnergyCost, 0, maxSubWeaponEnergy);
    }

    public void PickupSubweapon(SubWeapon _subweapon)
    {
        subweapon = _subweapon;
        subWeaponEnergyCost = SetSubWeaponEnergyCost();
    }

    public void PickupWeapon(BladeType _bladeType)
    {
        bladeType = _bladeType;
    }

    public void GetKey()
    {
        hasKey = true;
    }

    private int SetSubWeaponEnergyCost()
    {
        switch (subweapon)
        {
            case SubWeapon.rock:
                return 1;
            case SubWeapon.dart:
                return 1;
            case SubWeapon.mambele:
                return 2;
            case SubWeapon.boomerang:
                return 2;
            case SubWeapon.scythe:
                return 2;
            default:
                return 1;
        }
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        for (int i = 0; i < flashesPerSecond * iFramesDuration; i++)
        {
            sprite.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(iFramesDuration / (flashesPerSecond * iFramesDuration * 2));
            sprite.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (flashesPerSecond * iFramesDuration * 2));
        }
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }
}
