# Camera2DemoApp
Dead simple Xamarin app to run a camera on Android API 21-31 using Camera2 API and save the result

## Features:
- Pinch to zoom
- Front or back camera
- Little thumbnail to show the last captured image
- Saves location data with the image if user permits it.

## Gotchas:
- We're using Xamarin.Essentials for permissions and Xamarin.Community toolkit for the camera view. Dependency service is overkill at this point unless we want to do API specific stuff like HDR settings, or finer control than just "Zoom"
- Saving image files, as with most Android native stuff, is way more complicated than one would prefer it to be. I have a few devices lying around and it just so happens my daily driver (API 29) does this differently than my backup phone (API 28).  
  This app handles both, and hopefully everything before and after.
- My dev environment is very different to most Xamarin dev machines. I'm building and deploying in Linux, using JetBrains Rider, rather than the typical windows and Visual Studio setup. It works really well, but is limited to Android API 31 and no newer simply because Mono.Android SDK is not maintained as eagerly as I'd like.  
  Currently I can't build Android native code beyond C# 8.0 without running into strangenesses, Though PCL code works up to C# 10.
- I'm currently not handling rotation. I probably should, but... you know.
- API 28 and lower save the pic to a different folder than API 29 and above. No reason except it's just a little annoyance and I'm all about MVP at the moment.

## Notes:
- If, as I suspect, we want to do something like scanning QR or barcodes in our app, there's a pretty nice and mercifully up-to-date nuget package for that:  
  [https://github.com/JimmyPun610/BarcodeScanner.Mobile](https://github.com/JimmyPun610/BarcodeScanner.Mobile)  
  I mention this mainly because I've used it and can vouch for it. It also works faster on my device than the system camera app when pointed at a QR.  
  It's a wrapper for Google Vision APIs so it could probably be extended to do lots of interesting things like read text off street signs.
- I was delighted to find that the pinch numbers we get correspond *perfectly* with the camera zoom level without any kind of transformation needed.
- I used nuget packages for as much as I could. Some might call it laziness, but honestly I'd only write it the same or worse. There was a situation not long ago when I needed an android specific permission that was not included in Xamarin.Essentials (Detect activity? Something like that), so I had to do the whole rigmarole with turning event based APIs into async/await style methods using DependencyService and some clever setting of Task results upon received broadcasts... Anyway, today I'm just getting it done.