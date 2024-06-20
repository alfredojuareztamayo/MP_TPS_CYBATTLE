using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;
using Cinemachine;
using Photon.Pun;
using UnityEngine.UI;

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

    private GameObject testForWeapons;

    private Image weaponIcon;
    private Text ammoAmount;
    public Sprite[] weaponsIcons;
    public int[] ammoWeapons;

    public GameObject[] muzzelFlash;

    private string shooterName;
    private string gotShotName;
    public float[] damageAmts;

    public bool isDead = false;
    private GameObject choosePanel;
    private GameObject Mazes;

    private void Awake()
    {
       
    }
    // Start is called before the first frame update
    void Start()
    {
        /*
        Mazes = GameObject.Find("mazeGenerator");
        if (this.gameObject.GetComponent<PhotonView>().Owner.IsMasterClient == true)
        {
            Mazes.GetComponent<StackMaze>().isMaze = false;
        }
        else
        {
            Mazes.GetComponent<StackMaze>().isMaze = true;
        }*/
        choosePanel = GameObject.Find("ChoosePanel");
        weaponIcon = GameObject.Find("WeaponUI").GetComponent<Image>();
        ammoAmount = GameObject.Find("AmmoAmt").GetComponent<Text>();
        camObject = GameObject.Find("PlayerCam");
        
        ammoWeapons[0] = 60;
        ammoWeapons[1] = 0;
        ammoWeapons[2] = 0;
        ammoAmount.text = ammoWeapons[0].ToString();
        //aimTarget = GameObject.Find("AimRef").transform;
        if (this.gameObject.GetComponent<PhotonView>().IsMine == true)
        {
            cam = camObject.GetComponent<CinemachineVirtualCamera>();
            cam.Follow = this.gameObject.transform;
            cam.LookAt = this.gameObject.transform;
            //Invoke("SetLookAt", 0.1f);
            
        }
        else
        {
            this.gameObject.GetComponent<PlayerMovement>().enabled = false;
        }

        testForWeapons = GameObject.Find("Weapon1Pickup(clone)");
        if(testForWeapons == null) 
        {
            if(this.gameObject.GetComponent<PhotonView>().Owner.IsMasterClient == true)
            {
                
                   
                    var Spawner = GameObject.Find("SpawnScript");
                    Spawner.GetComponent<SpawnCharacters>().SpawnWeaponsStart();
   
            }
        }
    }

    /*void SetLookAt()
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
    }*/
    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(0) && isDead == false && choosePanel.activeInHierarchy == false)
        {
            if(this.gameObject.GetComponent<PhotonView>().IsMine == true && ammoWeapons[weaponNumber] > 0)
            {
                ammoWeapons[weaponNumber]--;
                ammoAmount.text = ammoWeapons[weaponNumber].ToString();
                GetComponent<DisplayColor>().PlayGunShot(GetComponent<PhotonView>().Owner.NickName, weaponNumber);
                this.GetComponent<PhotonView>().RPC("GunMuzzleFlash", RpcTarget.All);

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                if (Physics.Raycast(ray, out hit, 500))
                {
                    if (hit.transform.gameObject.GetComponent<PhotonView>() != null)
                    {
                        gotShotName = hit.transform.gameObject.GetComponent<PhotonView>().Owner.NickName;
                        

                    }
                    if (hit.transform.gameObject.GetComponent<DisplayColor>() != null)
                    {
                        hit.transform.gameObject.GetComponent<DisplayColor>().DeliverDamage(this.GetComponent<PhotonView>().Owner.NickName, hit.transform.gameObject.GetComponent<PhotonView>().Owner.NickName, damageAmts[weaponNumber]);
                    }
                    shooterName = GetComponent<PhotonView>().Owner.NickName;
                    Debug.Log(gotShotName + " got hit by " + shooterName);
                }
                this.gameObject.layer = LayerMask.NameToLayer("Default");

            }
        }
        if(Input.GetMouseButtonDown(1) && this.gameObject.GetComponent<PhotonView>().IsMine == true && isDead == false)
        {
            //weaponNumber++;
            this.GetComponent<PhotonView>().RPC("Change",RpcTarget.AllBuffered);
            if (weaponNumber > weapons.Length - 1)
            {
                weaponIcon.GetComponent<Image>().sprite = weaponsIcons[0];
                ammoAmount.text = ammoWeapons[0].ToString();
                weaponNumber = 0;
            }
                for(int i = 0; i < weapons.Length; i++)
            {
                weapons[i].SetActive(false);
            }
            weapons[weaponNumber].SetActive(true);
            weaponIcon.GetComponent<Image>().sprite = weaponsIcons[weaponNumber];
            ammoAmount.text = ammoWeapons[weaponNumber].ToString();
            leftHand.data.target = leftTarget[weaponNumber];
            rightHand.data.target = rightTarget[weaponNumber];
            leftThumb.data.target = thumbTarget[weaponNumber];
            rig.Build();
        }
    }

    public void UpdatePickup()
    {
        ammoAmount.text = ammoWeapons[weaponNumber].ToString();
    }

    [PunRPC]
    void GunMuzzleFlash()
    {
       
        muzzelFlash[weaponNumber].SetActive(true);
        StartCoroutine(MuzzleOff());
    }

    [PunRPC]
    public void Change() 
    {
        weaponNumber++;
       
        if (weaponNumber > weapons.Length - 1) weaponNumber = 0;
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[weaponNumber].SetActive(true);
        leftHand.data.target = leftTarget[weaponNumber];
        rightHand.data.target = rightTarget[weaponNumber];
        leftThumb.data.target = thumbTarget[weaponNumber];
        rig.Build();
    }

    IEnumerator MuzzleOff()
    {
        yield return new WaitForSeconds(0.03f);
        this.GetComponent<PhotonView>().RPC("MuzzleFlashOff",RpcTarget.All);
        //muzzelFlash[weaponNumber].SetActive(false);
    }

    [PunRPC]
    void MuzzleFlashOff()
    {
        muzzelFlash[weaponNumber].SetActive(false);
    }

    IEnumerator weaponsSpawnerDelay()
    {
        yield return new WaitForSeconds(3f);
        var Spawner = GameObject.Find("SpawnScript");
        Spawner.GetComponent<SpawnCharacters>().SpawnWeaponsStart();
    }
}
