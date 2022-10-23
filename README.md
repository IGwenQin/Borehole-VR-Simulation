# Borehole VR Simulation
A VR simulation project that simulates the underground borehole condition and registers images for 3D reconstruction.

## Prologue
This project aims to reconstruct 3D models of underground boreholes by means of Structure from Motion. However, it is challenging to generate 3D models using images registered inside an object. 
Therefore, I decided to come up with this project to simulate the borehole condition and register images. 



# What It Is
To achieve accurate comparative experiments under the same conditions, I employee Virtual Reality (VR) simulation for a borehole environment to investigate the appropriate camera layout and operation for generating high-quality 3D models by SfM.
This VR simulation of boreholes can determinate what is the proper camera movement so that the 3D model could be generated successfully. 



# How It Works
The camera has three movement styles.
1.	Register top-view images
2.	Register side-view images rotationally
3.	Register side-view images directly 



# Usage
Clone this project and open it with Unity.
## Generate a custom borehole 
1. Select Create/CylinderData file.

2. Setup parameters for underground borehole.
    - **material**  
      the texture of borehole wall
    - **circle resolution**  
      the number of vertices of circle meshes
    - **height resolution**  
      the number of vertices of height meshes
    - **spatial resolution** [m/vert]  
      the length per vertex
    - **smooth texture**  
      refine uv-mesh in connection with vertices
    - **smooth normal**  
      refine normal vector in connection of vertices
    - **generate zenith**  
      if true, generate zenith (top of the borehole)

<p></p>

3. Click the "Save" and then click the "Generate" button to generate borehole.  
  Note: If parameters are changed, click the "Update" button. 


## Generate custom camera layout
1. Select Create/CylinderCameraData file

2. Set parameters for camera layout
    - **cylinder data**  
      target borehole data asset for registering images
    - **position offset** [m]  
      the offset of camera arrangement (0 m means same height with zenith)
    - **sampling resolution** [/s]  
      shooting resolution per second (frame per second)
    - **camera velocity** [m/s]  
      the falling velocity of the camera
    - **camera rotation velocity** [euler/s]  
      the rotation velocity of the camera (euler angle)
    - **render size**  
      output image size
    - **render path**  
      output images path

<p></p>

3. Click the "Save" and "Generate" button to generate cameras.
Note: If parameters are changed, click the "Update" button.


4. Click the "Render" button to register images
Now you can generate 3D model with the registered images. 



