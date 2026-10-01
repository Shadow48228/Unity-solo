using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public PlayerController player;

    public GameObject PauseMenu;

    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI clipText;
    public TextMeshProUGUI ammoText;

    public Image healthBar;

    public bool paused = false; // for pause menu
    public bool enemiesGone = false; //enemy count
    public int enemyCount = 0; //enemy count

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1; // for pause menu

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            weaponName = GameObject.Find("weaponName").GetComponent<TextMeshProUGUI>();
            clipText = GameObject.Find("clipText").GetComponent<TextMeshProUGUI>();
            ammoText = GameObject.Find("ammoText").GetComponent<TextMeshProUGUI>();

            healthBar = GameObject.Find("healthBar").GetComponent<Image>();

            PauseMenu = GameObject.FindGameObjectWithTag("Pause"); // for pause menu

            Cursor.visible = false; // for pause menu
            Cursor.lockState = CursorLockMode.Locked; // for pause menu

            PauseMenu.SetActive(true); // for pause menu

            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length; //enemy count
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (paused) //for pause menu
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                Time.timeScale = 0;

                PauseMenu.SetActive(true);
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                Time.timeScale = 1;

                PauseMenu.SetActive(true);
            }

            healthBar.fillAmount = (float)player.health / (float)player.maxHealth;

            if (player.currentWeapon)
            {

                weaponName.text = player.currentWeapon.name;
                clipText.text = "Clip: " + player.currentWeapon.clip + '/' + player.currentWeapon.clipSize;
                ammoText.text = "Ammo: " + player.currentWeapon.ammo + '/' + player.currentWeapon.maxAmmo;

            }
            else
            {
                weaponName.text = "";
                clipText.text = "";
                ammoText.text = "";
            }

            if (enemyCount <= 0)
            {
                enemiesGone = true;
            }


        }
    }


    public void Pause()
    {
        paused = !paused;

        Cursor.visible = paused;

        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }

        PauseMenu.SetActive(paused);
    }

    public void LoadLevel(int levelID)
    {
        if (levelID >= SceneManager.sceneCount)
            Debug.Log("Scene ID too high: " + levelID);
        else
            SceneManager.LoadScene(levelID);
    }

    public void MainMenu()
    {
        LoadLevel(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}