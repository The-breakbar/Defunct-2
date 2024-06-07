using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id = 0;
    public Renderer visibilityObject;
    public TrailRenderer trail;
    private int points = 0;
    public Controls controls;
    private Checkpoint checkpointGoal;
    public Transform checkpoinMarker;

    private BgMusic bgMusic;

    [Range(0.0f, 1.0f)]
    public float trailWidth;

    public void Start()
    {
        bgMusic = FindObjectOfType<BgMusic>();
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
        bool onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;

        return onScreen && visibilityObject.isVisible;
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

    public void SetCheckpointGoal(Checkpoint checkpoint)
    {
        checkpointGoal = checkpoint;
    }

}
