using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    public Controls player1Controls;
    public Controls player2Controls;
    public Player player1;
    public Player player2;

    private ProgressBar player1Bar;
    private ProgressBar player2Bar;

    private VisualElement player1Points;
    private VisualElement player2Points;

    private VisualElement root;

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        player1Bar = root.Q<ProgressBar>("Player1_Boost");
        player2Bar = root.Q<ProgressBar>("Player2_Boost");

        player1Points = root.Q<VisualElement>("Player1_Points_Holder");
        player2Points = root.Q<VisualElement>("Player2_Points_Holder");
    }

    public void FixedUpdate()
    {
        player1Bar.value = player1Controls.GetBoostPercentage();
        player2Bar.value = player2Controls.GetBoostPercentage();

        for (int i = 0; i < player1Points.Children().Count(); i++)
        {
            player1Points.Children().ElementAt(i).SetEnabled(i < player1.GetPoints());
            player2Points.Children().ElementAt(i).SetEnabled(i < player2.GetPoints());
        }
    }
}