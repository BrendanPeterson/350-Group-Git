using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

    //[SerializeField]
   // RectTransform thrusterFuelAmount;

    [SerializeField]
    GameObject pauseMenu;

    private PlayerController1 controller;

    public void SetController(PlayerController1 _controller)
    {
        controller = _controller;
    }

    void Start()
    {
        PauseMenu.IsOn = false;
    }
    void Update()
    {
        //SetFuelAmount (controller.GetThrusterFuelAmount());  

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;
    }

    //void SetFuelAmount(float _amount)
    //{
       // thrusterFuelAmount.localScale = new Vector3(1f, _amount, 1f);
   // }
}
