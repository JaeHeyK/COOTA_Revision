using UnityEngine;
using Cinemachine;

public class CameraManager : SingletonNotDestroyed<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera[] vcamsInScene;
    protected CameraManager() {}
    private void FindVcam()
    {
        vcamsInScene = Resources.FindObjectsOfTypeAll<CinemachineVirtualCamera>();
    }

    public void SetCamera()
    {
        if (PlayerController.Instance is null) return;
        
        FindVcam();

        foreach (var vcam in vcamsInScene)
        {
            if (vcam.gameObject.CompareTag("MainVCam"))
            {
                vcam.Follow = PlayerController.Instance.gameObject.transform;
            }
        }
    }

    public void SetSubCamera()
    {
        foreach (var vcam in vcamsInScene)
        {
            if (vcam.gameObject.CompareTag("SubVCam"))
            {
                vcam.Follow = PlayerController.Instance.gameObject.transform;
            }
        }
    }

}

