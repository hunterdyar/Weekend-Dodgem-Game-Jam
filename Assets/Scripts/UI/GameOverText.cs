using System.Collections;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameOverText : MonoBehaviour
{
    public GameplayManager Manager;
    private TMP_Text _text;
    public AnimationCurve InSwing;
    public AnimationCurve OutSwing;
    public float offscreenPos;
    public float moveTime;
    public float stillTime;
    [FormerlySerializedAs("shouts")] public string[] badShouts;
    public string[] goodShouts;
    public string[] highscoreShouts;

    public float goodTimeThreshold = 20;
    private int _lastUsedShoutIndex;
    private static readonly string _lastUsedShoutPlayerPrefKey = "LastUsedShout";
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _text.enabled = false;
        _lastUsedShoutIndex = PlayerPrefs.GetInt(_lastUsedShoutPlayerPrefKey, -1);
        goodTimeThreshold = UnityEngine.Random.Range(-3, 3) + goodTimeThreshold;//gotta keep em guessing, ya know?
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
        var shouts = badShouts;
        if (Manager.IsHighscore)
        {
            shouts = highscoreShouts;
        }else if (Manager.SurvivalTime > goodTimeThreshold)
        {
            shouts = goodShouts;
        }
        
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

        _lastUsedShoutIndex = randIndex;
        PlayerPrefs.SetInt(_lastUsedShoutPlayerPrefKey,_lastUsedShoutIndex);
        return shouts[randIndex];
    }
}
