using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayableDirectorController : MonoBehaviour
{
    private GameObject player;
    private GameObject cameraManager;
    private SignalReceiver playerSignalReceiver;
    private Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        cameraManager = GameObject.Find("CameraManager");
        BindPlayerSignal();
    }

    private void BindPlayerSignal()
    {
        var director = GetComponent<PlayableDirector>();
        var timeline = director.playableAsset as TimelineAsset;

        foreach (var track in timeline.GetOutputTracks())
        {
            if (track.name == "Player Movement Signal")
            {
                director.SetGenericBinding(track, player.GetComponent<SignalReceiver>());
            } else if (track.name == "Player Animation")
            {
                director.SetGenericBinding(track, player.GetComponent<Animator>());
            } else if (track.name == "Player Tween")
            {
                director.SetGenericBinding(track, player.GetComponent<Transform>());
            } else if (track.name == "Camera Set Signal")
            {
                director.SetGenericBinding(track, cameraManager.GetComponent<SignalReceiver>());
            }
        }
    }
}
