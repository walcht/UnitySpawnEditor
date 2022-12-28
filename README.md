## About
A simple custom editor inspector script for editing spawn\waypoint positions.

This custom inspector is built using the new, powerful Unity UIToolkit. Thus this serves as an educational example on how to use this new UI package.

Spawn positions are stored in a ```List<Vector3>```container in an instance of a ScriptableObject called ```SpawnDatabase```.

![custom editor inspector image][editor-image]
________________________________________________________
When an instance of the ScriptableObject ```SpawnDatabase``` is selected in the Project window, __selected__ spawns from the list in the custom inspector are shown in the scene view. For instance spawns 1, 4, 6 and 9 are selected and they are shown in the scene view.

![scene view image][scene-view-image]

## Installation
To keep things simple, all you need to do is: 
1. Make sure that you use __Unity 2021.3__ or __newer version__
1. copy ```SpawnDatabase.cs``` and ```SpawnCustomInspector.cs``` into your ```Scripts``` folder

## Usage
1. Create a ```SpawnDatabase``` instance throught the assets menu ```Create/SO/Spawn Database```
2. Select the previously created instance of ```SpawnDatabase``` ScriptableObject
3. Spawn positions appear in the scene view as movable objects. You can now use the scene view with the custom inspector to properly place your spawn positions

## TODO
* Add new spawn positions using the scene view (by pressing Ctrl-D on a selected spawn)
* Add ```Copy to Clipboard``` button. When clicked, the spawn positions list is serialized into JSON and the resulting string is copied to the clipboard.
* Change wireframe cube colors to ```Green```
* Enhance layout\styles.

## LICENSE
MIT License.


[editor-image]: /images/custom-editor.PNG
[scene-view-image]: /images/scene-view.PNG
