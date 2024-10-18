//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponPreviewManager : MonoBehaviour
{

    // Variables
    private Sprite Weapon_Preview_Image_Sprite;

    // References
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Damage;
    [SerializeField] private TextMeshProUGUI ROF;
    [SerializeField] private TextMeshProUGUI InAccuracy;
    [SerializeField] private TextMeshProUGUI Description;
    [SerializeField] private UnityEngine.UI.Image Preview_Image;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshPreview(GameObject weapon)
    {

        // Gather weapon stagts
        WeaponStatInfo weapon_Info = weapon.GetComponent<WeaponStatInfo>();
        TurretWeapon weapon_Turret = weapon.GetComponent<TurretWeapon>();
        Projectile weapon_Turret_Projectile_Projectile = weapon.GetComponent<TurretWeapon>().projectile.GetComponent<Projectile>();

        // A projectile component doesn't exist it might be a missle to find that instead
        if (weapon_Turret_Projectile_Projectile == null)
        {
            Missile weapon_Turret_Projectile_Missile = weapon.GetComponent<TurretWeapon>().projectile.GetComponent<Missile>();

            // Display weapon stats
            Name.text = weapon_Info.Name;
            Damage.text = $"Damage: {weapon_Turret_Projectile_Missile.damage.ToString()}";
            ROF.text = $"Rate of Fire: {weapon_Turret.startTimeBtwShots / 0.1}";
            InAccuracy.text = $"Inaccuracy: Seeking";
            Description.text = weapon_Info.Description;
            Preview_Image.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        }
        else
        {
            // Display weapon stats
            Name.text = weapon_Info.Name;
            Damage.text = $"Damage: {weapon_Turret_Projectile_Projectile.damage.ToString()}";
            ROF.text = $"Rate of Fire: {weapon_Turret.startTimeBtwShots / 0.1}";
            InAccuracy.text = $"Inaccuracy: {weapon_Info.InAccuracy.ToString()}";
            Description.text = weapon_Info.Description;
            Preview_Image.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        }

        

    }
}
