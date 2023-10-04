using UnityEngine;
using UnityEngine.UI;

public class UIStatus : MonoBehaviour
{
    [SerializeField] private PlayerCondition playerCondition;
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private Image currentWeapon;
    [SerializeField] private Sprite[] subweapons;
    [SerializeField] private Text weaponEnergy;
    [SerializeField] private Image key;

    private void Update()
    {
        playerHealthBar.fillAmount = playerCondition.health / 17;

        currentWeapon.sprite = subweapons[(int)playerCondition.subweapon];
        currentWeapon.SetNativeSize();

        weaponEnergy.text = playerCondition.subWeaponEnergy.ToString();

        key.enabled = playerCondition.hasKey;
    }
}
