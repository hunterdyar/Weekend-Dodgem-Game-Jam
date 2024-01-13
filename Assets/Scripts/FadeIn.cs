
using UnityEngine;
using Blooper.TransitionEffects;
public class FadeIn : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Transition.TransitionInToScene(TransitionType.CircleWipe, 0.1f, 0.85f, Color.black));
    }
}
