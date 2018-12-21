# Traffic Simulator
Simulates traffic within cities

Importing OpenStreetMaps data
1. Export file from OSM and change file from .osm to .txt
2. Place file in Assets/Resources in Unity project
3. Create folder in Assets/Assets/Maps to store the road meshes
4. Open ParseMapData scene in Unity and select the Map object
5. In the Map Reader component type in the name of the .txt file (excluding .txt) under Resource File
6. In the Store Map component type in the name of the folder for your road meshes under Folder Name
7. Run the scene and wait until the log prints "Completed Road Rendering"
8. Select Game view and press F12
9. Wait until the log prints "Successfully stored map"
10. The map prefab is now in Assets/Assets/Maps and the individual roads in the folder created in step 3

Traffic Simulation
1. Place map prefab in new scene and remove the Road Maker component
2. Reset position of map to 0,0,0
3. Attach Graph Loader and Populate Streets component from Assets/MapScripts
4. Drag Graph Loader from map into the Loader slot in the Populate Streets component
5. Drag car prefab from Assets/Assets/TrafficPrefabs into Car Prefab slot in Populate Streets
6. In Populate Streets component input the number of cars for the simulation under Car Population
7. Run the scene and wait (The number of cars may affect the wait time. Cars will need a few moments to orient themselves in the right direction)