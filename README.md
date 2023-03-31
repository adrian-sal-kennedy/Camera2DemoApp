# Camera2DemoApp
Dead simple Xamarin app to run a camera on Android API 21-31 using Camera2 API and save the result

## Features:
- Pinch to zoom
- Front or back camera
- Little thumbnail to show the last captured image

## Gotchas:
- We're using Xamarin.Essentials for permissions and Xamarin.Community toolkit for the camera view. Dependency service is overkill at this point unless we want to do API specific stuff like HDR settings, or finer control than just "Zoom"
- Saving the file isn't implemented yet
