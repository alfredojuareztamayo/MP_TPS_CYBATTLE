using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Cinemachine;
using Photon.Pun;

public class WeaponChangeAdvance : MonoBehaviour
{
    public TwoBoneIKConstraint leftHand, rightHand, leftThumb;
    public RigBuilder rig;
    public Transform[] leftTarget, rightTarget, thumbTarget;
    public GameObject[] weapons;
    private int weaponNumber = 0;


    private CinemachineVirtualCamera cam;
    private GameObject camObject;

    public MultiAimConstraint[] aimObjects;
    private Transform aimTarget;
    // Start is called before the first frame update
    void Start()
    {

        camObject = GameObject.Find("PlayerCam");
        aimTarget = GameObject.Find("AimRef").transform;
        if (this.gameObject.GetComponent<PhotonView>().IsMine == true)
        {
            cam = camObject.GetComponent<CinemachineVirtualCamera>();
            cam.Follow = this.gameObject.transform;
            cam.LookAt = this.gameObject.transform;
            Invoke("SetLookAt", 0.1f);
        }
    }

    void SetLookAt()
    {
        if(aimTarget!= null)
        {
            for(int i = 0; i< aimObjects.Length; i++)
            {
                var target = aimObjects[i].data.sourceObjects;
                target.SetTransform(0, aimTarget.transform);
                aimObjects[i].data.sourceObjects = target;
            }
            rig.Build();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            weaponNumber++;
            if (weaponNumber > weapons.Length - 1) weaponNumber = 0;
            for(int i = 0; i < weapons.Length; i++)
            {
                weapons[i].SetActive(false);
            }
            weapons[weaponNumber].SetActive(true);
            leftHand.data.target = leftTarget[weaponNumber];
            rightHand.data.target = rightTarget[weaponNumber];
            leftThumb.data.target = thumbTarget[weaponNumber];
            rig.Build();
        }
    }
}
