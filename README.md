# LoSV-WorldGenProject
World Generation design and example implementation for our current game-development project in Unity3D.

## Goal and Purpose
The aim is to provide an easy to implement, high-level interface for randomly generating tile-based worlds with a few prerequisites. For our project, it needs to be able to serialize/deserialize the end result, a List\<Zone\>, back and forth from JSON. A progress bar will be implemented with little care for how it affects the speed of world-gen; any slowdown will be offset with a better user experience. The project is available in C# only.

The purpose in sharing this code is the hope that it may help another team with their own implementation of random world generation and/or serialization.

## Example Implementation
Generates, saves, and loads WorldData. When generating and loading WorldData, an example interpreter will run and instantiate simple tiles; this is just to display progress and make debugging easier when implementing your own.

## How to use
* Unzip the folder given and open either the project folder in unity, or import the .unitypackage into an existing project.
* Using the create menu, create and fill out a ZoneTypesList specific to your game: Create a new ZoneList for each ZoneType you specify in the zone list, and create a bunch of tiles for use with each ZoneList.
* When you want to generate a world, call `WorldManager.Generate()`, feeding your ZoneTypesList and ZoneLists as parameters where appropriate. If you have a progress bar you want to use, be sure to give the function a reference to that as well.

See the ExampleImplementation.cs and Example Scene for an example setup.

## Project Wiki
You can find more specific info on the classes and design used in the project [here](https://github.com/JYandev/LoSV-WorldGenProject/wiki).

## Credits and Acknowledgements
* Credit to Aron Granberg for his Unity3D-friendly build of JsonFX: https://bitbucket.org/TowerOfBricks/jsonfx-for-unity3d-git
* Thanks to jsonfx for the original JsonFX: https://github.com/jsonfx/jsonfx
* Unity Community for Singleton base class: http://wiki.unity3d.com/index.php/Singleton

## License
This example project is provided under the [MIT License](https://github.com/JYandev/LoSV-WorldGenProject/edit/master/LICENSE.txt "Link to the License").
