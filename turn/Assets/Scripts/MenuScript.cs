using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public static bool MenuActive = false;

    public GameObject MenuUI;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q))
        {
            if (MenuActive)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
	}

    public void CloseMenu()
    {
        MenuUI.SetActive(false);
        Time.timeScale = 1f;
        MenuActive = false;
    }
    public void OpenMenu()
    {
        MenuUI.SetActive(true);
        Time.timeScale = 0f;
        MenuActive = true;
    }
    public void GoToMainMenu()
    {

    }
    public void InventoryMenu()
    {

    }

}
