using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StopItems : MonoBehaviourPunCallbacks
{
    private Collider itemCollider;
    private Renderer itemRenderer;
    public float timeStop = 5;
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

                other.GetComponent<PhotonView>().RPC("StopMovementItem", RpcTarget.Others, timeStop);

                // Desactivar el ítem temporalmente
                this.GetComponent<PhotonView>().RPC("DeactivateItemStop", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void DeactivateItemStop()
    {
        itemCollider.enabled = false;
        itemRenderer.enabled = false;
        StartCoroutine(ReactivateItemStop());
    }

    private IEnumerator ReactivateItemStop()
    {
        yield return new WaitForSeconds(10f); // Tiempo que el ítem permanece desactivado
        this.GetComponent<PhotonView>().RPC("ActivateItemStop", RpcTarget.All);
    }
    [PunRPC]
    private void ActivateItemStop()
    {
        itemCollider.enabled = true;
        itemRenderer.enabled = true;
    }
}
