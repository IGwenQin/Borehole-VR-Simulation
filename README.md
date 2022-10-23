# Borehole VR Simulation
A VR simulation project that simulates the underground borehole condition and registers images for 3D reconstruction.

## Prologue
This project aims to reconstruct 3D models of underground boreholes by means of Structure from Motion. However, it is challenging to generate 3D models using images registered inside an object. 
Therefore, I decided to come up with this project to simulate the borehole condition and register images. 



# Installation
Clone this project and open with Unity.


# Sample
1. Open Assets/Sample/SampleScene.unity
2. Click "Render" botton in Assets/Sample/SampleCylinder.asset (wait for several seconds)
3. Images will be generated in output/


# Usage
## Generation of custom cylinder
1. Right click on Unity and select Create/CylinderData

2. Set parameters for generation of cylinder
    - **material**  
      the material of cylinder
    - **circle resolution**  
      the number of vertices of circle meshes
    - **height resolution**  
      the number of vertices of height meshes
    - **spatial resolution** [m/vert]  
      the length per one vertex
    - **smooth texture**  
      refine uv-mesh in connection of vertices
    - **sommth normal**  
      refine normal vector in connection of vertices
    - **generate zenith**  
      if true, generate zenith (top of cylinder)

<p></p>

3. Click "Save" and "Generate" botton to generate cylinder object  
  Note that: Game object in scene and asset correspond to each other. Therefore, if you want to create the other, create new CylinderData.

4. If parameters change, click "Update" botton

## Generation of custom camera arrangement
1. Right click on Unity and select Create/CylinderCameraData

2. Set parameters for camera arrangement
    - **cylinder data**  
      target cylinder data asset for shooting
    - **position offset** [m]  
      the offset of camera arrangement (0 m means same height with zenith)
    - **sampling resolution** [/s]  
      shooting resolution per second (frame per second)
    - **camera velocity** [m/s]  
      the falling velocity of camera
    - **camera rotation velocity** [euler/s]  
      the rotation velocity of camera (euler angle)
    - **render size**  
      output image size
    - **render path**  
      output images path

<p></p>

3. Click "Save" and "Generate" botton to generate camera objects  
  Note that: Semilar to cylinder, game object in scene and asset correspond to each other. Therefore, if you want to create the other, create new CylinderCameraData.

4. If parameters change, click "Update" botton

5. Click "Render" botton to shoot images  
  It may be several seconds. The output images will be generated in render path.


