using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class PistolVariables : MonoBehaviour
{
    [SerializeField] AudioSource gunSound;
    [SerializeField] Camera cam;

    public float fireRate = 0.5f;
    public int clipSize = 8;
    public int reservedAmmoCapacity = 72;
    public float range = 2;

    [SerializeField] bool canShoot; //DO NOT CHANGE VARIABLES
    [SerializeField] int currentAmmo;//DO NOT CHANGE VARIABLES
    [SerializeField] int ammoInReserved;//DO NOT CHANGE VARIABLES

    public Image muzzleFlash;
    public Sprite[] flashes;

    public Vector3 normalPosition;
    public Vector3 aimingPosition;
    public float aimSmoothing = 7;

    public TextMeshProUGUI ammoCount;

    void Start()
    {
        gunSound.GetComponent<AudioSource>();
        gunSound.enabled = false;
        currentAmmo = clipSize;
        ammoInReserved = reservedAmmoCapacity;
        canShoot = true;
    }
    void Update()
    {
        UI();
        DetermineAim();
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot && currentAmmo > 0)
        {
            canShoot = false;
            currentAmmo--;
            StartCoroutine(ShootGun());
        }
        else if (Input.GetKeyDown(KeyCode.R) && currentAmmo < clipSize && ammoInReserved > 0)
        {
            int ammoNeeded = clipSize - currentAmmo;
            if (ammoNeeded >= ammoInReserved)
            {
                currentAmmo += ammoInReserved;
                ammoInReserved -= ammoNeeded;
            }
            else
            {
                currentAmmo = clipSize;
                ammoInReserved -= ammoNeeded;

            }
        }
    }
    void DetermineAim()
    {
        Vector3 target = normalPosition;
        if (Input.GetMouseButton(1))
        {
            target = aimingPosition;
        }
        Vector3 desieredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);
        transform.localPosition = desieredPosition;
    }
    void Recoil()
    {
        transform.localPosition -= Vector3.forward * 0.1f;
    }
    IEnumerator ShootGun()
    {
        Recoil();
        gunSound.enabled = true;
        gunSound.Play();
        RayCastZombie();
        StartCoroutine(MuzzleFlash());
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
        gunSound.enabled = false;
    }
    IEnumerator MuzzleFlash()
    {
        muzzleFlash.sprite = flashes[Random.Range(0, flashes.Length)];
        muzzleFlash.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.sprite = null;
        muzzleFlash.color = new Color(0, 0, 0, 0);
    }
    void RayCastZombie()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 1 << LayerMask.NameToLayer("Zombie")))
        {
            try
            {
                Debug.Log("Hit Zombie");
                Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.None;
                rb.AddForce(transform.forward * 500);
            }
            catch
            {

            }
        }
    }
    public void UI()
    {
        ammoCount.SetText(currentAmmo + "/" + ammoInReserved);
    }
}