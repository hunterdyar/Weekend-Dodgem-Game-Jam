using System.Collections;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class GameOverText : MonoBehaviour
{
    private TMP_Text _text;
    public AnimationCurve InSwing;
    public AnimationCurve OutSwing;
    public float offscreenPos;
    public float moveTime;
    public float stillTime;
    public string[] shouts;
    private int _lastUsedShoutIndex;
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _text.enabled = false;
    }

    void OnEnable()
    {
        GameplayManager.OnGameStateChange+= OnGameStateChange;
    }

    void OnDisable()
    {
        GameplayManager.OnGameStateChange -= OnGameStateChange;
    }

    private void OnGameStateChange(GameState state)
    {
        if (state == GameState.GameOver)
        {
            StartCoroutine(GameOverAnimation());
        }
        else
        {
            _text.enabled = false;
        }
    }

    private IEnumerator GameOverAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        _text.enabled = true;
        _text.SetText(GetShout());
        Vector3 start = new Vector3(offscreenPos, 0, 0);
        Vector3 end = Vector3.zero;
        _text.transform.localPosition = start;
        float t = 0;
        while (t < 1)
        {
            _text.transform.localPosition = Vector3.Lerp(start, end, InSwing.Evaluate(t));
            t += Time.deltaTime / moveTime;
            yield return null;
        }

        yield return new WaitForSeconds(stillTime);

        t = 0;
        start = _text.transform.localPosition;
        end = new Vector3(-offscreenPos, 0, 0);
        while (t < 1)
        {
            _text.transform.localPosition = Vector3.Lerp(start, end, OutSwing.Evaluate(t));
            t += Time.deltaTime / moveTime;
            yield return null;
        }
    }

    private string GetShout()
    {
        if (shouts.Length == 0)
        {
            return "oof!";
        }else if(shouts.Length == 1)
        {
            return shouts[0];
        }

        int randIndex = _lastUsedShoutIndex;
        while (randIndex == _lastUsedShoutIndex)
        {
            randIndex = Random.Range(0, shouts.Length);
        }

        return shouts[randIndex];
    }
}
