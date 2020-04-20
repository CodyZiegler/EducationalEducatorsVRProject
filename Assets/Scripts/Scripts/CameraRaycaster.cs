using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CameraRaycaster : MonoBehaviour
{
    private bool AllowFog = false;
    private bool FogOff;
    void Update()
    {
        RaycastHit hit;
        Ray r = new Ray(transform.position, transform.forward);
        int layer = 1 << 8;
        Debug.DrawRay(r.origin, r.direction, Color.blue);


        layer = ~layer;
        if (Physics.Raycast(r, out hit, 10, layer))
        {
            Sign s = hit.transform.GetComponent<Sign>();
            if (s != null)
            {
                s.LookingAt();
            }
        }
    }
    private void OnPreRender()
    {
        if (SceneManager.GetActiveScene().name == "Negative")
        {
            FogOff = RenderSettings.fog;
            RenderSettings.fog = true;
            RenderSettings.fogDensity = .01f;
        }
        else
        {
            FogOff = RenderSettings.fog;
            RenderSettings.fog = AllowFog;
        }
    }
    private void OnPostRender()
    {
        if (SceneManager.GetActiveScene().name == "Negative")
        {
            RenderSettings.fog = true;
            RenderSettings.fogDensity = .0001f;
        }
        else
        {
            RenderSettings.fog = FogOff;
        }
    }
}
