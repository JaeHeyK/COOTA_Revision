using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayableDirectorController : MonoBehaviour
{
    private SignalReceiver playerSignalReceiver;
    private Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        playerSignalReceiver = GameObject.FindWithTag("Player").GetComponent<SignalReceiver>();
        playerAnimator = GameObject.FindWithTag("Player").GetComponentInChildren<Animator>();
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
                director.SetGenericBinding(track, playerSignalReceiver);
            } else if (track.name == "Player Animation")
            {
                director.SetGenericBinding(track, playerAnimator);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled && collision.CompareTag("Player"))
        {
            GetComponent<PlayableDirector>().Play();
        }
    }
}
