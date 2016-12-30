# LoSV-WorldGenProject
World Generation design and example implementation for our current game-development project in Unity3D.

## Goal and Purpose
The aim is to provide an easy to implement, high-level interface for randomly generating tile-based worlds with a few prerequisites. For our project, it needs to be able to serialize/deserialize the end result, a List\<Zone\>, back and forth from JSON. The project is available in C# only.

The purpose in sharing this code is the hope that it may help another team with their own implementation of random world generation and/or serialization.

## Example Implementation
Generates, saves, and loads WorldData. When generating and loading WorldData, an example interpreter will run and instantiate simple tiles; this is just to display progress and make debugging easier when implementing your own.

## How to use
*(Unfinished)*

## API Reference
*(Unfinished)*

## Credits and Acknowledgements
* Credit to Aron Granberg for his Unity3D-friendly build of JsonFX: https://bitbucket.org/TowerOfBricks/jsonfx-for-unity3d-git
* Thanks to jsonfx for the original JsonFX: https://github.com/jsonfx/jsonfx

## License
This example project is provided under the [MIT License](https://github.com/JYandev/LoSV-WorldGenProject/edit/master/LICENSE.txt "Link to the License").
