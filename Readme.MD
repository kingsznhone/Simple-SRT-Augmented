# Simple-SRT-Augmented

A simple SRT subtitle parser with basic progress control.

Augmented from [roguecode/Unity-Simple-SRT](https://github.com/roguecode/Unity-Simple-SRT)

It'll parse most sane SRT's, and will display them in a `Text` element, crossfading between lines depending on a `Fade Time` you set.

Third Party Plugin Required : [DOTween](http://dotween.demigiant.com/)

## To use it

- Add the `Subtitle Displayer` component to something in the world. 
- Create two `Text` UI elements, both the same, and drag their references to the `Subtitle Displayer` component.
- Rename your `.srt` file to `.txt`, then drag it to the `Subtitle` field on `Subtitle Displayer`.
- Call `Prepare(TextAsset _Sub, TimeSpan Progress, double VideoLength)` to Prepare the subtitle coroutine. 
- Call `SubStart()` to start a prepared subtitle coroutine.
- Call `SubStop()` to stop subtitle coroutine.

A sub file like this:
```
1
00:00:00,000 --> 00:00:02,500
Mary had

2
00:00:02,500 --> 00:00:04,500
a little <i>lamb</i>

3
00:00:04,500 --> 00:00:06,500
little <b>lamb</b>

4
00:00:06,500 --> 00:00:08,500
little <size=40>lamb</size>

5
00:00:09,000 --> 00:00:12,000
<b><i><color=red>Until she didn't.</color></i></b>
```
Will result in this:

![Totally the best gif evar](https://github.com/roguecode/Unity-Simple-SRT/blob/master/Preview.gif?raw=true)

A good program to make subs is [SubtitleEdit](https://github.com/SubtitleEdit/subtitleedit/releases)

# API

## Prepare(TextAsset _Sub, TimeSpan Progress, double VideoLength)

Load Subtitle Asset and set to specific progress time.

There is no pause or continue function.

Call `SubStop()` at pause.

Then prepare a new Coroutine in current progress time.

Call `SubStart()` at continue.

<br/>

### Parameters

```_Sub``` TextAsset

Subtitle Asset

```Progress``` TimeSpan

Current Video Progress time.

```VideoLength``` double

Total Length of video in seconds.

<br/>

## SubStart()

Start prepared subtitle coroutine.

<br/>

## SubStop()

Stop Current subtitle coroutine.

<br/>
