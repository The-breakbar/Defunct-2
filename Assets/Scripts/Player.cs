using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id = 0;
    public GameObject model1;
    public GameObject model2;
    public GameObject model3;

    public PlayerColorChanger model1Color;
    public PlayerColorChanger model2Color;
    public PlayerColorChanger model3Color;

    private int points = 0;
    public Controls controls;
    private Checkpoint checkpointGoal;
    public Transform checkpoinMarker;

    private BgMusic bgMusic;

    public ParticleSystem deathParticles;

    public TrailRenderer trail;
    [Range(0.0f, 1.0f)]
    public float trailWidth;

    public void Start()
    {
        bgMusic = FindObjectOfType<BgMusic>();
        SwitchToModel(1);
    }

    public void Update()
    {
        // Update trail width based on the music spectrum
        trail.widthMultiplier = trailWidth * bgMusic.GetSpectrum();
        if (controls.IsBoosting()) trail.widthMultiplier *= 2.0f;

        if (checkpointGoal != null)
        {
            if (!checkpoinMarker.gameObject.activeSelf) checkpoinMarker.gameObject.SetActive(true);

            // Calculate angle between player and checkpoint and rotate the checkpoint marker
            Vector3 playerToCheckpoint = checkpointGoal.transform.position - transform.position;
            playerToCheckpoint.y = 0.0f;
            float angle = Vector3.SignedAngle(playerToCheckpoint, transform.forward, Vector3.up);
            checkpoinMarker.rotation = Quaternion.Euler(0.0f, -angle, 0.0f);
        }
        else
        {
            if (checkpoinMarker.gameObject.activeSelf) checkpoinMarker.gameObject.SetActive(false);
        }
    }

    public int GetId()
    {
        return id;
    }

    // Checks if the player is visible on the screen
    public bool CheckVisibility()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        return screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;
    }

    public void ClearTrail()
    {
        trail.Clear();
    }

    public void AddPoint()
    {
        points++;
    }

    public int GetPoints()
    {
        return points;
    }

    public void ResetPoints()
    {
        points = 0;
    }

    public void SetCheckpointGoal(Checkpoint checkpoint)
    {
        checkpointGoal = checkpoint;
    }

    public void SwitchToModel1()
    {
        SwitchToModel(1);
    }

    public void SwitchToModel2()
    {
        SwitchToModel(2);
    }

    public void SwitchToModel3()
    {
        SwitchToModel(3);
    }

    private int activeModel = 1;
    public int GetModel()
    {
        return activeModel;
    }
    public void SwitchToModel(int model)
    {
        activeModel = model;
        model1.SetActive(model == 1);
        model2.SetActive(model == 2);
        model3.SetActive(model == 3);
    }

    public void SetMainColor(Color color)
    {
        mainColor = color;
        model1Color.SetMainColor(color);
        model2Color.SetMainColor(color);
        model3Color.SetMainColor(color);
    }

    public void SetSecondaryColor(Color color)
    {
        secondaryColor = color;
        model1Color.SetSecondaryColor(color);
        model2Color.SetSecondaryColor(color);
        model3Color.SetSecondaryColor(color);

        trail.startColor = color;
    }

    private Color mainColor;
    public Color GetMainColor()
    {
        return mainColor;
    }
    private Color secondaryColor;
    public Color GetSecondaryColor()
    {
        return secondaryColor;
    }
    public void PlayDeathEffect()
    {
        deathParticles.Play();
    }

}
