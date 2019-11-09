using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float mFireRate = .5f;
    public float mFireRange = 50f;
    public float mHitForce = 100f;
    public int mLaserDamage = 100;

    private LineRenderer mLaserLine; // Line that will represent the Laser

    private bool mLaserLineEnabled; // Define if the laser line is showing

    private WaitForSeconds mLaserDuration = new WaitForSeconds(0.05f); //Time that laser lines show on screen

    private float mNextfire; //time until next fire

    private void Fire() {

        RaycastHit hit;

        Transform cam = Camera.main.transform;

        mNextfire = Time.time + mFireRate;

        Vector3 rayOrigin = cam.position;

        StartCoroutine(LaserFX());

        if (Physics.Raycast(rayOrigin, cam.forward, out hit, mFireRange))
        {
            mLaserLine.SetPosition(1, hit.point);

            CubeBehave cubeCtr = hit.collider.GetComponent<CubeBehave>();
            if (cubeCtr != null) {
                if (hit.rigidbody != null) {
                    hit.rigidbody.AddForce(-hit.normal * mHitForce);
                    cubeCtr.Hit(mLaserDamage);
                }
            }
        }
        else {
            mLaserLine.SetPosition(1, cam.forward * mFireRange);
        }


        mLaserLine.SetPosition(0, transform.up * -10f);
    }


    // Start is called before the first frame update
    void Start()
    {
        mLaserLine = GetComponent<LineRenderer>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > mNextfire) {
            Fire();
        }

    }

    private IEnumerator LaserFX()
    {
        mLaserLine.enabled = true;

        yield return mLaserDuration;
        mLaserLine.enabled = false;
    }

}
