using System;
using UnityEngine;

public class Fractal : MonoBehaviour
{

	[SerializeField, Range(1, 8)] int depth = 4;

	[SerializeField] Mesh mesh;

	[SerializeField] Material material;
	
	struct FractalPart {
		public Vector3 direction;
		public Quaternion rotation;
		public Transform transform;
	}
	
	FractalPart[][] parts;
	
	static Vector3[] directions = {
		Vector3.up, Vector3.right, Vector3.left, Vector3.forward, Vector3.back
	};

	static Quaternion[] rotations = {
		Quaternion.identity,
		Quaternion.Euler(0f, 0f, -90f), Quaternion.Euler(0f, 0f, 90f),
		Quaternion.Euler(90f, 0f, 0f), Quaternion.Euler(-90f, 0f, 0f)
	};
	
	FractalPart  CreatePart (int levelIndex, int childIndex, float scale) {
		var go = new GameObject("Fractal Part L" + levelIndex + " C" + childIndex);
		go.transform.SetParent(transform, false);
		go.AddComponent<MeshFilter>().mesh = mesh;
		go.AddComponent<MeshRenderer>().material = material;
		return new FractalPart {
			direction = directions[childIndex],
			rotation = rotations[childIndex],
			transform = go.transform
		};
	}

	private void Awake()
	{
		parts = new FractalPart[depth][];
		// root object only one
		int length = 1;
		for (int i = 0; i < parts.Length; i++) {
			parts[i] = new FractalPart[length];
			// each part has 5 children, so *= 5
			length *= 5;
		}
		
		float scale = 1f;
		CreatePart(0, 0, scale);
		// li代表层级
		for (int li = 1; li < parts.Length; li++)
		{
			scale /= 0.5f;
			FractalPart[] levelParts = parts[li];
			// levelParts.Length代表每个层级有多少个物体。
			// fpi代表一个父物体的所有子物体（一共五个儿子）
			for (int fpi = 0; fpi < levelParts.Length; fpi += 5) {
				for (int ci = 0; ci < 5; ci++) {
					// ci遍历一个父物体的所有子物体
					CreatePart(li, ci, scale);
				}
			}
		}
	}
}