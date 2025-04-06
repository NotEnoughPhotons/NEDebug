![Logo](https://github.com/Not-Enough-Photons/NEDebug/blob/master/img/NEDebugLogo.png)

# Introduction
**NEDebug** is a debugging library geared towards mod developers of BONELAB. Whether you are a puritan using the SDK, or a Lava Gang code monkey, NEDebug is for you.

This library aims to make the process of debugging mods much easier, by providing creators with the ability to visualize what is happening under the curtains.

# Usage
NEDebug is a simple library that shouldn't require much documentation to explain, which is one of the goals of the project. Each function name should tell you exactly what it does. Anything not exposed publicly is marked as internal.
## Functions
### Drawing
- ``NEDraw.DrawLine``
- ``NEDraw.DrawRay``
- ``NEDraw.DrawPlane``
- ``NEDraw.DrawBox``
- ``NEDraw.DrawDisc``
- ``NEDraw.DrawCylinder``
- ``NEDraw.DrawSphere``
### Logging
- ``NELog.Log``
- ``NELog.Warning``
- ``NELog.Error``
## Recommendations
If you plan to use this library in your code, it is recommended you work in a debug build of your project. Using NEDebug in release builds may result in bad mod performance.

If you have to, wrap your NEDebug calls inside of a preprocessor block like so:
```cs
#if DEBUG
  using NEP.NEDebug;
#endif

// Some code later...
#if DEBUG
  NEDraw.DrawSphere(transform.position, Quaternion.identity, Color.white, radius: 1.0f);
  NEDraw.DrawRay(new Ray(transform.position, transform.forward), Color.red);
#endif
```
