using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{

    public float swayIntensity;
    public float smoothSway;

    private Quaternion origin_rotation;

    void Start()
    {
        origin_rotation = transform.rotation;
    }

    void Update()
    {
        UpdateSway();
    }

    void UpdateSway()
    {
        float sway_x_mouse = Input.GetAxis("Mouse X");
        float sway_y_mouse = Input.GetAxis("Mouse Y");

        Quaternion target_x_ajustment = Quaternion.AngleAxis(-swayIntensity * sway_x_mouse, Vector3.up);
        Quaternion target_y_ajustment = Quaternion.AngleAxis(swayIntensity * sway_y_mouse, Vector3.right);
        Quaternion target_rotation = origin_rotation * target_x_ajustment * target_y_ajustment;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, target_rotation, Time.deltaTime * smoothSway);
    }

}
