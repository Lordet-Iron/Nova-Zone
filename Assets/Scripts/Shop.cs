using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    // Variables
    private List<GameObject> hardPoint_Previews = new List<GameObject>();
    [SerializeField] private GameObject selected_Weapon;
    [SerializeField] private UpgradeInfo selected_Upgrade;
    // Upgrades:  1 = Default
    private int healthUpgrade = 1;
    private List<GameObject> selectedWeapons = new List<GameObject>();

    private int weaponMax = 0;
    private int weaponCount = 0;

    private int spentScore = 0;

    // Reference
    [SerializeField] PlayerManager playerManager;
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject SelectedShip; // Default to be set in inspector
    [SerializeField] private TextMeshProUGUI SlotTracker_Text;
    [SerializeField] private Image Preview;
    [SerializeField] private GameObject HardPoint;
    [SerializeField] private WeaponPreviewManager WPM; // Weapon Preview Manager
    [SerializeField] private UpgradePreviewManager UPM;
    [SerializeField] private HullPreviewManager HBM;
    [SerializeField] private GameObject SelectedButton;
    [SerializeField] private GameObject defaultSelectedShopSection;
    [SerializeField] private List<GameObject> shopSections = new List<GameObject>();
    [SerializeField] private Score score;
    [SerializeField] private List<GameObject> UpgradeButtons; //
    [SerializeField] private GameObject Victory;
    
    private Canvas Shop_Canvas;
    
    //private GameObject PlayerObject;
    //private Player PlayerScript;

    

    // Start is called before the first frame update
    void Start()
    {
        Shop_Canvas = GetComponent<Canvas>();
        //PlayerObject = GameObject.FindGameObjectWithTag("Player");
        //PlayerScript = PlayerObject.GetComponent<Player>();
        weaponMax = SelectedShip.GetComponent<Player>().weapon_Placements.Count;
        SlotTracker_Text.text = $"Weapons: 0/{weaponMax}";

        UIUpdate();
        SelectShopSection(defaultSelectedShopSection);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) // When player presses "O". Open the shop
        {
            if (Panel.activeSelf)
            {
                Panel.SetActive(false);
            }
            else
            {
                Panel.SetActive(true);
            }
            //AudioListener.volume = 0f;
        }
    }

    public void SubmitUpgrades() // Spawn the player with the upgrades applied as well as the enemies.
    {
        score.setScore(0);
        playerManager.SpawnPlayer(SelectedShip, healthUpgrade, selectedWeapons);
        playerManager.SpawnEnemies();

        

        foreach (GameObject upgradeButton in UpgradeButtons)
        {
            if (upgradeButton.GetComponent<UpgradeInfo>().isPurchased == true)
            {
                if (upgradeButton.GetComponent<UpgradeInfo>().type == "Health")
                {
                    healthUpgrade -= upgradeButton.GetComponent<UpgradeInfo>().modif;
                }
                upgradeButton.GetComponent<ButtonGraphicController>().ToggleAlpha();
                upgradeButton.GetComponent<UpgradeInfo>().isPurchased = false;
                
            }
        }

        // Clear Weapons
        Debug.Log("Clear");
        selectedWeapons.Clear();
        UIUpdate();

        Panel.SetActive(false); // Close shop when finished
        Victory.SetActive(false);
    }

    private void UIUpdate()
    {
        weaponCount = selectedWeapons.Count;
        SlotTracker_Text.text = $"Weapons: {weaponCount}/{weaponMax}";

        // Update Preview
        SpriteRenderer selectedShip_SpriteRend = SelectedShip.GetComponent<SpriteRenderer>();
        Debug.Log(selectedShip_SpriteRend.sprite);
        Preview.sprite = selectedShip_SpriteRend.sprite;
        Preview.color = Color.white;
        Preview.SetNativeSize();

        foreach (GameObject hardpoint in hardPoint_Previews)
        {
            Destroy(hardpoint);
        }
        hardPoint_Previews.Clear();
        foreach (GameObject weapon in selectedWeapons)
        {
            hardPoint_Previews.Add(Instantiate(HardPoint, Preview.transform));
            int lastIndex = hardPoint_Previews.Count - 1;
            hardPoint_Previews[lastIndex].GetComponent<Image>().sprite = weapon.GetComponent<SpriteRenderer>().sprite;
            hardPoint_Previews[lastIndex].GetComponent<Image>().SetNativeSize();
            hardPoint_Previews[lastIndex].GetComponent<RectTransform>().position += SelectedShip.GetComponent<Player>().weapon_Placements[lastIndex].position * 200;
        }

        /*// Preview ship
        // Spawn objects as inactive
        GameObject created_player = Instantiate(SelectedShip, Preview); // Spawn in ship
        //created_HUD = Instantiate(new_player_canvas, gameObject.transform); // Spawn in HUD

        // Player Component
        Player created_Player_Player = created_player.GetComponent<Player>();
        created_Player_Player.health = 1;
        //created_Player_Player.Health_Bar = created_HUD.transform.GetChild(0).gameObject;

        // Player Weapons
        created_Player_Player.weapons = selectedWeapons;

        // Resise object
        Transform created_player_transform = created_player.GetComponent<Transform>();
        created_player_transform.localScale = new Vector3(120, 120, 1);*/


    }

    public void SelectShip(GameObject ship)
    {
        SelectedShip = ship;
        weaponMax = SelectedShip.GetComponent<Player>().weapon_Placements.Count;
        UIUpdate();
        HullInfo hullInfo = ship.GetComponent<HullInfo>();
        HBM.RefreshPreview(hullInfo);



        /*// Display weapons
        List<Transform> weapon_locations = new List<Transform>();
        weapon_locations = SelectedShip.GetComponent<Player>().weapon_Placements;*/

        /*// This madness of editting spites and textures is really not something I've ever enjoyed doing. Why am I doing this to myself while drinking alcohol
        foreach (GameObject weapon in selectedWeapons)
        {
            // Grab the textures
            Texture2D mainTexture = Preview.sprite.texture;
            Texture2D weaponTexture = weapon.GetComponent<SpriteRenderer>().sprite.texture;
            // Make new texture
            Texture2D combinedTexture = new Texture2D(mainTexture.width, mainTexture.height);
            // Copy main onto combined
            combinedTexture.SetPixels(mainTexture.GetPixels());

            // Get the size of the sprite2 texture
            int width2 = weaponTexture.width;
            int height2 = weaponTexture.height;

            // Calculate the position to draw sprite2 in the center of the new texture
            int x = (mainTexture.width - width2) / 2;
            int y = (mainTexture.height - height2) / 2;

            // Copy the pixels from sprite2 to the new texture at the correct position
            Graphics.CopyTexture(weaponTexture, 0, 0, 0, 0, width2, height2, combinedTexture, 0, 0, x, y);

            // Apply the changes to the new texture
            combinedTexture.Apply();

            // Assign the new texture to a sprite
            Sprite combinedSprite = Sprite.Create(combinedTexture, new Rect(0, 0, combinedTexture.width, combinedTexture.height), new Vector2(0.5f, 0.5f));

            selectedShip_SpriteRend.sprite = combinedSprite;
        }*/

        // Attempt 2 to bodge the issue with prefabs
        // Clear list first


    }

    public void AddWeapon() // Massive WIP, modulairty doesn't exist
    {
        if (weaponCount < weaponMax)
        {
            if (selected_Weapon.GetComponent<WeaponStatInfo>().Price <= score.ScoreTotal)
            {
                selectedWeapons.Add(selected_Weapon);
                score.DecreaseScore(selected_Weapon.GetComponent<WeaponStatInfo>().Price);
                spentScore += selected_Weapon.GetComponent<WeaponStatInfo>().Price;
            }
            
            
        }
        UIUpdate();
    }
    public void RemoveWeapon() // Massive WIP, modulairty doesn't exist
    {
        GameObject foundWeapon = selectedWeapons.FirstOrDefault(x => x.gameObject == selected_Weapon);
        if (foundWeapon != null)
        {
            selectedWeapons.Remove(foundWeapon);
            score.IncreaseScore(selected_Weapon.GetComponent<WeaponStatInfo>().Price);
        }
        
        UIUpdate();
    }

    public void HealthUpgrade1(Image Button_Image)
    {
        if (healthUpgrade == 1)
        {
            healthUpgrade = 2;
            Button_Image.color = new Color(1, 1, 1, (float)0.5);
        }
        else if (healthUpgrade == 2)
        {
            healthUpgrade = 1;
            Button_Image.color = new Color(1, 1, 1, (float)0.71);
        }

    }

    public void EnableMenu(GameObject menu)
    {
        menu.SetActive(true);
    }
    public void DisableMenu(GameObject menu)
    {
        menu.SetActive(false);
    }
    public void SelectWeapon(GameObject weapon)
    {
        selected_Weapon = weapon;
        WPM.RefreshPreview(weapon);
    }
    public void SelectUpgrade(UpgradeInfo upgrade)
    {


        selected_Upgrade = upgrade;
        UPM.RefreshPreview(upgrade);

    }
    public void SelectUpgrade(GameObject upgradeButton)
    {

        UpgradeInfo upgrade = upgradeButton.GetComponent<UpgradeInfo>();

        SelectedButton = upgradeButton;
        selected_Upgrade = upgrade;
        UPM.RefreshPreview(upgrade);

    }

    public void AddUpgrade()
    {
        if (selected_Upgrade.isPurchased == false && score.ScoreTotal >= selected_Upgrade.price)
        {
            if (selected_Upgrade.type == "Health")
            {
                healthUpgrade += selected_Upgrade.modif;
            }
            SelectedButton.GetComponent<ButtonGraphicController>().ToggleAlpha();
            selected_Upgrade.isPurchased = true;
            score.DecreaseScore(selected_Upgrade.price);
        }
        
    }
    public void RemoveUpgrade()
    {
        
        if (selected_Upgrade.isPurchased == true)
        {
            if (selected_Upgrade.type == "Health")
            {
                healthUpgrade -= selected_Upgrade.modif;
            }
            SelectedButton.GetComponent<ButtonGraphicController>().ToggleAlpha();
            selected_Upgrade.isPurchased = false;
            score.IncreaseScore(selected_Upgrade.price);
        }
    }

    public void SelectShopSection(GameObject selectedSection)
    {
        foreach (GameObject section in shopSections)
        {
            if (section != selectedSection)
            {
                section.SetActive(false);
            }
            else
            {
                section.SetActive(true);
            }
        }
    }

}
