﻿using UnityEngine;

public class Van7Windscreen : MonoBehaviour {

	[HideInInspector] public Mesh mesh;

	private float height;
	private float depth;
	private float rim;

	//Declare the script of the prevous mesh
	Van6Bonnet bonnet;

	void Start () {

		//Create a mesh filter while also assigning it as a variable to get the mesh property
		MeshFilter meshFilter = gameObject.AddComponent<MeshFilter> ();

		//Create a mesh renderer so we can actually display the mesh
		gameObject.AddComponent<MeshRenderer> ();

		//Set the mesh object to be that of the mesh from the mesh filter
		mesh = meshFilter.mesh;

		//Set a random material
		Object[] loadedMaterials = Resources.LoadAll("Materials");
		gameObject.GetComponent<Renderer> ().material = (Material)loadedMaterials [Random.Range (0, loadedMaterials.Length - 2)];

		//Get script of previous mesh
		bonnet = FindObjectOfType<Van6Bonnet>();

		//Create the mesh
		CreateMesh ();
	}
	
	void CreateMesh () {
		
		//Assign random values to the height, depth, and rims of the mesh
		RandomControl dc = FindObjectOfType<RandomControl> ();
		height = Random.Range (dc.vanModel.Windscreen.Height.x, dc.vanModel.Windscreen.Height.y);
		depth = Random.Range (dc.vanModel.Windscreen.Depth.x, dc.vanModel.Windscreen.Depth.y);
		rim = Random.Range (dc.vanModel.Windscreen.Rim.x, dc.vanModel.Windscreen.Rim.y);

		//height = Random.Range (1.76f, 3.23f);
		//depth = Random.Range (0.2f, 1.85f);
		//rim = Random.Range (0.1f, 0.2f);
		Vector3 previous2 = bonnet.mesh.vertices [2];
		Vector3 previous3 = bonnet.mesh.vertices [3];

		//Assign the mesh vertices
		mesh.vertices  = new Vector3[] { 

			new Vector3 (previous2.x, previous2.y, previous2.z),
			new Vector3 (previous3.x, previous3.y, previous3.z),
			new Vector3 (previous2.x + rim, previous2.y + rim, previous2.z + (rim/2)),
			new Vector3 (previous3.x - rim, previous2.y + rim, previous2.z + (rim/2)),
			new Vector3 (previous3.x, previous3.y + (height + rim), previous3.z + depth),
			new Vector3 (previous3.x - rim, previous3.y + height, previous3.z + (depth - rim)),
			new Vector3 (previous2.x , previous2.y + (height + rim), previous2.z + depth),
			new Vector3 (previous2.x + rim, previous2.y + height, previous2.z + (depth - rim))
		};

		//Assign the mesh triangles
		mesh.triangles = new int[] { 0,2,1, 2,3,1, 1,3,4, 4,3,5, 4,5,6, 6,5,7, 6,7,0, 7,2,0 };

		//Calculate the normals of the mesh fom the triangles
		mesh.RecalculateNormals ();
	}

	void Update () {

		//Draw some rays to represent the normals for debugging purposes
		for (int i = 0; i < mesh.vertices.Length - 1; i++) {

			Vector3 forward = transform.TransformDirection (mesh.normals [i]) * 2;
			Debug.DrawRay (mesh.vertices [i], forward, Color.green);
			Debug.DrawRay (mesh.vertices [i + 1], forward, Color.green);
			Debug.DrawRay (Vector3.Lerp (mesh.vertices [i], mesh.vertices [i + 1], 0.5f), forward, Color.green);
		}
	}
}
