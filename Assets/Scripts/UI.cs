using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CountdownElement
{
    private VisualElement element;
    private float appearTime;
    private float disappearTime;
    private float waitForTime;
    private float appear;
    private float disappear;
    private float waitFor;

    public CountdownElement(VisualElement element, float appearTime, float disappearTime, float waitForTime)
    {
        this.element = element;
        this.appearTime = appearTime;
        this.disappearTime = disappearTime;
        this.waitForTime = waitForTime;
        this.appear = 0.0f;
        this.disappear = 0.0f;
        this.waitFor = waitForTime;
    }

    public void Tick(float delta)
    {
        if (waitFor > 0.0f)
        {
            waitFor -= delta;
            if (waitFor < 0) appearTime -= -waitFor;
            return;
        }

        if (appear > 0.0f)
        {
            appear -= delta;
            appear = Mathf.Max(0.0f, appear);
            Appear(element, appear / appearTime);
        }
        else if (disappear > 0.0f)
        {
            disappear -= delta;
            disappear = Mathf.Max(0.0f, disappear);
            Disappear(element, disappear / disappearTime);
        }
    }

    private void Appear(VisualElement element, float t)
    {
        t = 1.0f - t;
        element.style.opacity = Mathf.Lerp(0.0f, 1.0f, t);

        // Make its size go from big to small
        float scale = Mathf.Lerp(3.0f, 1.0f, t);
        element.style.scale = new Scale { value = new Vector3(scale, scale, 1.0f) };
    }

    private void Disappear(VisualElement element, float t)
    {
        t = 1.0f - t;
        element.style.opacity = Mathf.Lerp(1.0f, 0.0f, t);
    }

    public void Reset()
    {
        appear = appearTime;
        disappear = disappearTime;
        waitFor = waitForTime;
    }
}

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

    private VisualElement container;

    private readonly float appearTime = 0.6f;
    private readonly float disappearTime = 0.3f;
    private List<CountdownElement> countdownElements = new List<CountdownElement>();
    private Label winText;

    private Label boostText1;
    private Label boostText2;

    public GameLoop gameLoop;

    private VisualElement root;

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        player1Bar = root.Q<ProgressBar>("Player1_Boost");
        player2Bar = root.Q<ProgressBar>("Player2_Boost");

        player1Points = root.Q<VisualElement>("Player1_Points_Holder");
        player2Points = root.Q<VisualElement>("Player2_Points_Holder");

        container = root.Q<VisualElement>("Container");
        winText = root.Q<Label>("win");

        countdownElements.Add(new CountdownElement(root.Q<VisualElement>("three"), appearTime, disappearTime, 1.0f - appearTime));
        countdownElements.Add(new CountdownElement(root.Q<VisualElement>("two"), appearTime, disappearTime, 2.0f - appearTime));
        countdownElements.Add(new CountdownElement(root.Q<VisualElement>("one"), appearTime, disappearTime, 3.0f - appearTime));
        countdownElements.Add(new CountdownElement(root.Q<VisualElement>("go"), appearTime, disappearTime, 4.0f - appearTime));

        boostText1 = root.Q<Label>("boostText1");
        boostText2 = root.Q<Label>("boostText2");
    }

    public void FixedUpdate()
    {
        player1Bar.Children().ElementAt(0).style.color = player1.GetSecondaryColor();
        player1Bar.value = player1Controls.GetBoostPercentage();

        player2Bar.Children().ElementAt(0).style.color = player2.GetSecondaryColor();
        player2Bar.value = player2Controls.GetBoostPercentage();

        for (int i = 0; i < player1Points.Children().Count(); i++)
        {
            if (i < player1.GetPoints())
            {
                player1Points.Children().ElementAt(i).style.backgroundColor = player1.GetSecondaryColor();
                player1Points.Children().ElementAt(i).SetEnabled(true);
            }
            else
            {
                player1Points.Children().ElementAt(i).SetEnabled(false);
            }

            if (i < player2.GetPoints())
            {
                player2Points.Children().ElementAt(i).style.backgroundColor = player2.GetSecondaryColor();
                player2Points.Children().ElementAt(i).SetEnabled(true);
            }
            else
            {
                player2Points.Children().ElementAt(i).SetEnabled(false);
            }
        }

        // Animate border color
        Color color = Color.Lerp(Color.red, Color.black, Mathf.PingPong(Time.fixedTime, 1));
        container.style.borderBottomColor = color;
        container.style.borderTopColor = color;
        container.style.borderLeftColor = color;
        container.style.borderRightColor = color;

        // Countdown
        for (int i = 0; i < countdownElements.Count; i++)
        {
            countdownElements[i].Tick(Time.fixedDeltaTime);
        }

        // if the player is boosting, move the label around on both the x and y axis
        if (player1Controls.IsBoosting() && gameLoop.GetState() == GameState.Racing)
        {
            // vary the offset between -10 and 10, based on fixedDeltatTIme
            float xOffset = Mathf.Sin(Time.fixedTime * 50) * 8;
            float yOffset = Mathf.Cos(Time.fixedTime * 40) * 8;

            // set the position
            boostText1.style.left = xOffset;
            boostText1.style.top = yOffset;
        }
        else
        {
            // reset the position
            boostText1.style.left = 0;
            boostText1.style.top = 0;
        }


        if (player2Controls.IsBoosting() && gameLoop.GetState() == GameState.Racing)
        {
            float xOffset = Mathf.Sin(Time.fixedTime * 50) * 8;
            float yOffset = Mathf.Cos(Time.fixedTime * 40) * 8;

            boostText2.style.right = xOffset;
            boostText2.style.top = yOffset;
        }
        else
        {
            boostText2.style.right = 0;
            boostText2.style.top = 0;
        }

    }

    public void PlayCountdown()
    {
        foreach (CountdownElement element in countdownElements)
        {
            element.Reset();
        }
    }

    public void ShowWin(int playerId)
    {
        winText.text = "Player  " + playerId + "  wins!";
        winText.style.opacity = 1.0f;
    }

    public void HideWin()
    {
        winText.style.opacity = 0.0f;
    }
}