using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class InvisibleItem : MonoBehaviourPunCallbacks
{
    private Collider itemCollider;
    private Renderer itemRenderer;
    public float timeInvisible = 5;
    // Start is called before the first frame update
    void Start()
    {

        itemCollider = GetComponent<Collider>();
        itemRenderer = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //PhotonView photonView = other.GetComponent<PhotonView>();
            //string name = GetComponent<PhotonView>().Owner.NickName;
            if (photonView != null && photonView.IsMine)
            {

                other.GetComponent<PhotonView>().RPC("InvisibleTime", RpcTarget.AllBuffered, timeInvisible);

                // Desactivar el ítem temporalmente
                this.GetComponent<PhotonView>().RPC("DeactivateInvi", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void DeactivateInvi()
    {
        itemCollider.enabled = false;
        itemRenderer.enabled = false;
        StartCoroutine(ReactivateInvi());
    }

    private IEnumerator ReactivateInvi()
    {
        yield return new WaitForSeconds(10f); // Tiempo que el ítem permanece desactivado
        this.GetComponent<PhotonView>().RPC("ActivateInvi", RpcTarget.All);
    }
    [PunRPC]
    private void ActivateInvi()
    {
        itemCollider.enabled = true;
        itemRenderer.enabled = true;
    }
}
