using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class WeaponShooting : MonoBehaviour
{
    private Camera cam;

    [SerializeField] float shootRange = 100f;

    [SerializeField] GameObject muzzleEffect;

    private Animator animator;

    private bool isShooting, readyToShoot;
    private bool allowReset = true;
    [SerializeField] float shootingDelay = 0.2f; 

    [SerializeField] int bulletsPerBurst = 3;
    private int burstBulletsLeft;
    private int bulletsLeft;

    [SerializeField] int magazineSize = 20;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    [SerializeField] Text ammoText;

    void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;

        bulletsLeft = magazineSize;

        cam = Camera.main;
        animator = GetComponent<Animator>();

        ammoText.text = "Ammo: " + magazineSize.ToString() + "/" + magazineSize.ToString();
    }

    void Update()
    {
        if(currentShootingMode == ShootingMode.Auto)
            isShooting = Input.GetKey(KeyCode.Mouse0);
        else isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        if(readyToShoot && isShooting)
        {
            if(bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                Shoot();
            }
            // else Play No Ammo Sound
        }
    }

    private void Shoot()
    {
        bulletsLeft--;

        readyToShoot = false;

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, shootRange))
        {
            Debug.Log(hit.transform.name);
        }

        muzzleEffect.GetComponent<VisualEffect>().Play();
        animator.SetTrigger("RECOIL");

        SoundManager.Instance.shootingSoundPistol.Play();

        if(allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if(currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("Shoot", shootingDelay);
        }

        ammoText.text = "Ammo: " + bulletsLeft.ToString() + "/" + magazineSize.ToString();
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
}
