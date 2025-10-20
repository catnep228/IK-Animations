Just whatch this video https://youtu.be/cg_vXWPaG8A
In short, simply transfer the code to your Unity project.
Next, add your model with the created animations (you can use any program, even the Mixamo website) to the scene. Using the Animation Rigging package, apply the Bone Render Setup and Rig Setup to the model. 
After the latter, you'll have a Rig object inside the model. Inside the Rig, create right and left arms and apply the Two-Bone IK Constraint and Multi-Rotation Constraint components to both, then configure them (more details in the video). 
Add the "EnvironmentInteractionStateMachine" script to the Rig and fill in the required fields.
The Collider component is redundant and completely unnecessary (it's simply used to create another Collider, and you can use the CharacterController in the EnvironmentInteractionStateMachine class to calculate the size of the new collider).
If you want to keep the field, don't forget to add the CapsuleSolider component to your model.
Good luck!
