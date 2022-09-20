using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    private Vector3 _thrusterPosition;
    private Vector3 _thrusterScale;
    public void SpeedBoostThrust()
    {
        _thrusterPosition = new Vector3(0, -4f, 0);
        _thrusterScale = new Vector3(0.5f, 1.5f, 0.5f);
        transform.localPosition = _thrusterPosition;
        transform.localScale = _thrusterScale;
    }

    public void NormalSpeedThrust()
    {
        _thrusterPosition = new Vector3(0, -2.5f, 0);
        _thrusterScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.localPosition = _thrusterPosition;
        transform.localScale = _thrusterScale;
    }
}
