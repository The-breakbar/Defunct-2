using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class PauseUI : MonoBehaviour
{
    public Object[] prefabs;
    public GameObject player1_model_holder;
    public GameObject player2_model_holder;

    public Player player1;
    public Player player2;

    private VisualElement root;
    private VisualElement pauseMenu;
    private SliderInt audioVolume;
    private Button resumeButton;
    
    private DropdownField Model_Dropdown_1;
    private DropdownField Model_Dropdown_2;
    
    // Color Player 1
    private SliderInt redSlider1;
    private SliderInt greenSlider1;
    private SliderInt blueSlider1;

    private SliderInt redSlider1_secondary;
    private SliderInt greenSlider1_secondary;
    private SliderInt blueSlider1_secondary;

    // Color Player 2
    private SliderInt redSlider2;
    private SliderInt greenSlider2;
    private SliderInt blueSlider2;

    private SliderInt redSlider2_secondary;
    private SliderInt greenSlider2_secondary;
    private SliderInt blueSlider2_secondary;


    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        pauseMenu = root.Q<VisualElement>("Pause_Menu");
        
        audioVolume = root.Q<SliderInt>("AudioVolume");
        audioVolume.RegisterCallback<ChangeEvent<int>>(evt => AudioListener.volume = evt.newValue / 100.0f);

        resumeButton = root.Q<Button>("Resume_Button");
        resumeButton.RegisterCallback<ClickEvent>(evt => Hide());

        Model_Dropdown_1 = root.Q<DropdownField>("Model_Dropdown_1");
        Model_Dropdown_1.choices = new List<string> { "Model 1", "Model 2", "Model 3" };
        Model_Dropdown_1.index = 0;
        Model_Dropdown_1.Children().ElementAt(1).style.backgroundColor = Color.black;
        Model_Dropdown_1.RegisterCallback<ChangeEvent<string>>(evt => SetCurrentModel(evt.newValue, 1));

        Model_Dropdown_2 = root.Q<DropdownField>("Model_Dropdown_2");
        Model_Dropdown_2.choices = new List<string> { "Model 1", "Model 2", "Model 3" };
        Model_Dropdown_2.index = 0;
        Model_Dropdown_2.Children().ElementAt(1).style.backgroundColor = Color.black;
        Model_Dropdown_2.RegisterCallback<ChangeEvent<string>>(evt => SetCurrentModel(evt.newValue, 2));

        // Color Player 1
        redSlider1 = root.Q<SliderInt>("P1_Color_Red");
        greenSlider1 = root.Q<SliderInt>("P1_Color_Green");
        blueSlider1 = root.Q<SliderInt>("P1_Color_Blue");

        redSlider1.RegisterCallback<ChangeEvent<int>>(evt =>
            SetCurrentColor(evt.newValue, greenSlider1.value, blueSlider1.value, 1));
        greenSlider1.RegisterCallback<ChangeEvent<int>>(evt =>
            SetCurrentColor(redSlider1.value, evt.newValue, blueSlider1.value, 1));
        blueSlider1.RegisterCallback<ChangeEvent<int>>(evt =>
            SetCurrentColor(redSlider1.value, greenSlider1.value, evt.newValue, 1));

        // Color Player 1 Secondary 
        redSlider1_secondary = root.Q<SliderInt>("P1_Color_Red_s");
        greenSlider1_secondary = root.Q<SliderInt>("P1_Color_Green_s");
        blueSlider1_secondary = root.Q<SliderInt>("P1_Color_Blue_s");

        redSlider1_secondary.RegisterCallback<ChangeEvent<int>>(evt =>
            SetCurrentColor(evt.newValue, greenSlider1_secondary.value, blueSlider1_secondary.value, 1, true));
        greenSlider1_secondary.RegisterCallback<ChangeEvent<int>>(evt =>
            SetCurrentColor(redSlider1_secondary.value, evt.newValue, blueSlider1_secondary.value, 1, true));
        blueSlider1_secondary.RegisterCallback<ChangeEvent<int>>(evt =>
            SetCurrentColor(redSlider1_secondary.value, greenSlider1_secondary.value, evt.newValue, 1, true));

        // Color Player 2
        redSlider2 = root.Q<SliderInt>("P2_Color_Red");
        greenSlider2 = root.Q<SliderInt>("P2_Color_Green");
        blueSlider2 = root.Q<SliderInt>("P2_Color_Blue");

        redSlider2.RegisterCallback<ChangeEvent<int>>(evt =>
            SetCurrentColor(evt.newValue, greenSlider2.value, blueSlider2.value, 2));
        greenSlider2.RegisterCallback<ChangeEvent<int>>(evt =>
            SetCurrentColor(redSlider2.value, evt.newValue, blueSlider2.value, 2));
        blueSlider2.RegisterCallback<ChangeEvent<int>>(evt =>
            SetCurrentColor(redSlider2.value, greenSlider2.value, evt.newValue, 2));

        // Color Player 2 Secondary
        redSlider2_secondary = root.Q<SliderInt>("P2_Color_Red_s");
        greenSlider2_secondary = root.Q<SliderInt>("P2_Color_Green_s");
        blueSlider2_secondary = root.Q<SliderInt>("P2_Color_Blue_s");

        redSlider2_secondary.RegisterCallback<ChangeEvent<int>>(evt =>
            SetCurrentColor(evt.newValue, greenSlider2_secondary.value, blueSlider2_secondary.value, 2, true));
        greenSlider2_secondary.RegisterCallback<ChangeEvent<int>>(evt =>
            SetCurrentColor(redSlider2_secondary.value, evt.newValue, blueSlider2_secondary.value, 2, true));
        blueSlider2_secondary.RegisterCallback<ChangeEvent<int>>(evt =>
            SetCurrentColor(redSlider2_secondary.value, greenSlider2_secondary.value, evt.newValue, 2, true));


        // SET DEFAULTS
        SetCurrentModel("Model 1", 1);
        SetCurrentModel("Model 1", 2);
        SetCurrentColor(255, 128, 0, 1, true);
        SetCurrentColor(255, 0, 255, 2, true);

        Hide();
    }

    public void Show()
    {
        pauseMenu.style.display = DisplayStyle.Flex;
        // root.visible = true;

        // Set the current model
        Model_Dropdown_1.index = player1.GetModel() - 1;
        Model_Dropdown_2.index = player2.GetModel() - 1;

        SetCurrentModel(Model_Dropdown_1.value, 1);
        SetCurrentModel(Model_Dropdown_2.value, 2);
        // SetCurrentModel(Model_Dropdown_2.choices.ElementAt(player2.GetModel() - 1), 2);
    }

    public void Hide()
    {
        pauseMenu.style.display = DisplayStyle.None;
    }

    private void SetCurrentModel(String model, int player)
    {
        int index = Model_Dropdown_1.choices.IndexOf(model);
        Debug.Log("Setting model " + model + " for player " + player);

        // Get the player model holder
        GameObject playerModelHolder = player == 1 ? player1_model_holder : player2_model_holder;

        // Destroy the current model
        foreach (Transform child in playerModelHolder.transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate the new model
        Instantiate(prefabs.ElementAt(index), playerModelHolder.transform.position,
            playerModelHolder.transform.rotation, playerModelHolder.transform);

        var player_go = player == 1 ? player1.gameObject : player2.gameObject;
        player_go.GetComponent<Player>().SwitchToModel(index + 1);

        // apply the color
        SetCurrentColor(redSlider1.value, greenSlider1.value, blueSlider1.value, 1);
        SetCurrentColor(redSlider1_secondary.value, greenSlider1_secondary.value, blueSlider1_secondary.value, 1, true);

        SetCurrentColor(redSlider2.value, greenSlider2.value, blueSlider2.value, 2);
        SetCurrentColor(redSlider2_secondary.value, greenSlider2_secondary.value, blueSlider2_secondary.value, 2, true);
    }

    private void SetCurrentColor(int red, int green, int blue, int player, bool secondary = false)
    {
        // Set the color
        var color = new Color(red / 255.0f, green / 255.0f, blue / 255.0f);
        var player_go = player == 1 ? player1.gameObject : player2.gameObject;
        if (secondary)
            player_go.GetComponent<Player>().SetSecondaryColor(color);
        else
            player_go.GetComponent<Player>().SetMainColor(color);

        // Get the player model holder
        GameObject playerModelHolder = player == 1 ? player1_model_holder : player2_model_holder;

        // Get the player model
        GameObject playerModel = playerModelHolder.transform.GetChild(0).gameObject;

        // Get the player color changer
        PlayerColorChanger playerColorChanger = playerModel.GetComponent<PlayerColorChanger>();

        if (secondary)
            playerColorChanger.SetSecondaryColor(color);
        else
            playerColorChanger.SetMainColor(color);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("PRESSING ESCAPE");
            if (pauseMenu.style.display == DisplayStyle.None)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }
}