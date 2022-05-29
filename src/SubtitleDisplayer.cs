using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleDisplayer : MonoBehaviour
{
    public IEnumerator SubtitleThread;
    [SerializeField] Text Text;
    [SerializeField] Text Text2;
    [SerializeField]
    public bool Show;
    [Range(0, 1)]
    public float FadeTime;

    TextAsset Subtitle;

    VideoStopwatch VideoProgressTimer;

    private void OnEnable()
    {
        var hide = Text.color;
        hide.a = 0;
        Text.color = hide;
        Text2.color = hide;
    }

    private void Update()
    {
        if (Show)
        {
            Text.gameObject.SetActive(true);
            Text2.gameObject.SetActive(true);
        }
        else
        {
            Text.gameObject.SetActive(false);
            Text2.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Start SRT subrourine with a given subtitle file
    /// </summary>
    /// <param name="_Sub">Subtitle Assets</param>
    /// <param name="Progress">Current frame position</param>
    /// <param name="_VideoLength">Video Length in seconds</param>
    /// <returns></returns>
    private IEnumerator Begin(TextAsset _Sub, TimeSpan Progress, double VideoLength)
    {
        Subtitle = _Sub;
        VideoProgressTimer = new VideoStopwatch(Progress);

        var currentlyDisplayingText = Text;
        var fadedOutText = Text2;

        currentlyDisplayingText.text = string.Empty;
        fadedOutText.text = string.Empty;

        currentlyDisplayingText.gameObject.SetActive(true);
        fadedOutText.gameObject.SetActive(true);

        yield return FadeTextOut(currentlyDisplayingText);
        yield return FadeTextOut(fadedOutText);

        var parser = new SRTParser(Subtitle);

        VideoProgressTimer.Start();
        SubtitleBlock currentSubtitle = null;
        while (true)
        {
            var subtitleblock = parser.GetForTime(VideoProgressTimer.Seconds);
            if (VideoProgressTimer.Seconds < VideoLength)
            {
                if (subtitleblock == null)
                {
                    var toColor = currentlyDisplayingText.color;
                    toColor.a = 0;
                    currentlyDisplayingText.color = toColor;
                    fadedOutText.color = toColor;
                }
                else
                {
                    if (!subtitleblock.Equals(currentSubtitle))
                    {
                        currentSubtitle = subtitleblock;

                        // Swap references around
                        var temp = currentlyDisplayingText;
                        currentlyDisplayingText = fadedOutText;
                        fadedOutText = temp;

                        // Switch subtitle text
                        currentlyDisplayingText.text = currentSubtitle.Text;
                        // And fade out the old one. Yield on this one to wait for the fade to finish before doing anything else.
                        StartCoroutine(FadeTextOut(fadedOutText));

                        // Yield a bit for the fade out to get part-way
                        yield return new WaitForSeconds(FadeTime / 3);

                        // Fade in the new current
                        yield return FadeTextIn(currentlyDisplayingText);
                    }
                }
                yield return null;
            }
            else
            {
                VideoProgressTimer.StartOffset = TimeSpan.FromSeconds(0);
                VideoProgressTimer.Reset();
                Debug.Log("Subtitle Finish");
                StartCoroutine(FadeTextOut(currentlyDisplayingText));
                yield return FadeTextOut(fadedOutText);
                currentlyDisplayingText.gameObject.SetActive(false);
                fadedOutText.gameObject.SetActive(false);
                yield break;
            }
        }
    }

    /// <summary>
    /// Load Subtitle Asset and get ready.
    /// There is no pause and continue function
    /// Call SubStop() at pause.
    /// Then Prepare a new Coroutine in current progress.
    /// Call SubStart() at continue.
    /// </summary>
    /// <param name="_Sub"></param>
    /// <param name="Progress"></param>
    /// <param name="VideoLength"></param>
    public void Prepare(TextAsset _Sub, TimeSpan Progress, double VideoLength)
    {
        SubtitleThread = Begin(_Sub, Progress, VideoLength);
    }

    /// <summary>
    /// Start Subtitle Coroutine.
    /// </summary>
    public void SubStart()
    {
        StartCoroutine(SubtitleThread);
    }

    public void SubStop()
    {
        StopCoroutine(SubtitleThread);
    }

    void OnValidate()
    {
        FadeTime = ((int)(FadeTime * 10)) / 10f;
    }

    IEnumerator FadeTextOut(Text text)
    {
        var toColor = text.color;
        toColor.a = 0;
        yield return Fade(text, toColor, Ease.OutSine);
    }

    IEnumerator FadeTextIn(Text text)
    {
        var toColor = text.color;
        toColor.a = 1;
        yield return Fade(text, toColor, Ease.InSine);
    }

    IEnumerator Fade(Text text, Color toColor, Ease ease)
    {
        yield return DOTween.To(() => text.color, color => text.color = color, toColor, FadeTime).SetEase(ease).WaitForCompletion();
    }
}
