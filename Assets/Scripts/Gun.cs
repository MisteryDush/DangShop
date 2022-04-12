using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    int damage = 1;
    float range = 100f;
    public Camera cam;
    public ParticleSystem particle;
    public Animator anim;
    float impactForce = 1000f;
    float fireRate = 0.75f;
    float totalTime;
    zombie zombie;
    private bool isPicked = false;
    public int ammoInMagazine = 0;
    public int ammoLeft = 0;
    AudioSource audio;
    public Text ammoState;
    [SerializeField] private LayerMask vision;
    private float soundSpread = 30f;
    [SerializeField] private GameObject hitParticle;
    private bool flashlightToggle = true;
    [SerializeField] private Light flashlight;
    private bool isReloading = false;
    private ParticleSystem.MainModule particleColor;
    private bool isShooting = false;

    private void Start()
    {
        flashlight.enabled = false;
        totalTime = Time.time;
        audio = gameObject.GetComponent<AudioSource>();
        particleColor = hitParticle.GetComponent<ParticleSystem>().main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && totalTime < Time.time && isPicked && ammoInMagazine > 0)
        {
            anim.SetTrigger("shoot");
            totalTime = Time.time + fireRate;
        }
        if (isPicked)
        {
            if (Input.GetKeyDown("f"))
            {
                flashlight.enabled = flashlightToggle;
                flashlightToggle = !flashlightToggle;
            }
            ammoState.text = ammoInMagazine + "/" + ammoLeft;
        }
        else
        {
            ammoState.enabled = false;
        }
        if (Input.GetKeyDown("r") && isPicked) ReloadAnim();
    }


    void Shoot()
    {
        StartCoroutine(IsShootingSwitcher());
        if (!isReloading)
        {
            Collider[] hitCollider = Physics.OverlapSphere(gameObject.transform.position, soundSpread);
            foreach (Collider obj in hitCollider)
            {
                try { obj.gameObject.GetComponent<zombie>().SoundHeard(gameObject.transform); }
                catch (System.NullReferenceException e) { }
            }
            ammoInMagazine -= 1;
            audio.Play();
            RaycastHit hit;
            particle.Play();
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range, ~vision);
            try
            {
                zombie = hit.transform.GetComponentInParent<zombie>();
            }
            catch (System.NullReferenceException e)
            {
                Debug.Log(e);

            }
            if (zombie != null)
            {
                particleColor.startColor = new Color(255f, 0f, 0f);
                GameObject hitGO = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitGO, 1f);
                zombie.TakeDamage(damage);
            }
            else
            {
                particleColor.startColor = new Color(255f, 170f, 0f);
                GameObject hitGO = Instantiate(hitParticle, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitGO, 1f);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }

    }


    public void ToggleReloading()
    {
        isReloading = !isReloading;
    }

    public void ReloadAnim()
    {
        if (ammoInMagazine == 8 || ammoLeft == 0)
        {
            return;
        }
        anim.SetTrigger("reload");
    }

    public void Reload()
    {
        if (ammoInMagazine == 8 || ammoLeft == 0)
        {
            return;
        }
        else if (ammoInMagazine < 8 && ammoLeft != 0)
        {
            int ammoToFill = 8 - ammoInMagazine;
            if(ammoToFill > ammoLeft)
            {
                ammoInMagazine += ammoLeft;
                ammoLeft = 0;
                return;
            }
            ammoInMagazine = 8;
            ammoLeft -= ammoToFill;
        }
    }

    public void PickedUp()
    {
        gameObject.transform.parent = cam.transform;
        if (gameObject.transform.parent.childCount > 1)
        {
            gameObject.SetActive(false);
        }
        gameObject.transform.localPosition = new Vector3(0.185f, -0.157f, 0.333f);
        gameObject.transform.localRotation = new Quaternion(90f, -.19f, 90f, 0f);
        ammoState.enabled = true;
        isPicked = true;
    }

    public void SwitchFromPistol()
    {
        isReloading = false;
        StopAllCoroutines();
    }

    private IEnumerator IsShootingSwitcher()
    {
        isShooting = true;
        yield return new WaitForSeconds(0.20f);
        isShooting = false;
    }

    public bool IsPicked 
    { 
        get { return isPicked; }
    }

    public bool IsShooting
    {
        get { return isShooting; }
    }

    public bool IsReloading
    {
        get { return isReloading; }
    }

}
