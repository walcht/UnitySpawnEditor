## About
A simple custom editor inspector script for editing spawn\waypoint positions.

## Installation
To keep things simple, all you need to do is copy ```SpawnDatabase.cs``` and ```SpawnCustomInspector.cs``` into your ```Scripts``` folder.

## Usage
1. Create a _SpawnDatabase_ instance throught the assets menu "Create/SO/Spawn Database"
2. Select the previously created instance of _SpawnDatabase_ scriptable object
3. Spawn positions appear in the scene view as movable objects. You can now use the scene view to properly place your spawn positions

## TODO
* Make spawn positions selectable 
* Make only selected spawn points movable (currently all spawn points are movable)
* Add new spawn positions using the scene view
* Use custom list property drawer for better visualization
* Selected spawn points in the custom inspector should also be selected in the scene view

## LICENSE
MIT License.
