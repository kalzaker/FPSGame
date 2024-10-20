using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.VFX;

public class WeaponShooting : MonoBehaviour
{
    private Camera cam;

    [SerializeField] float shootRange = 100f;

    [SerializeField] GameObject muzzleEffect;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, shootRange))
        {
            Debug.Log(hit.transform.name);
        }

        muzzleEffect.GetComponent<VisualEffect>().Play();
    }
}
