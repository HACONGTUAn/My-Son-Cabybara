using UnityEngine;
using System.Collections;

namespace Fishing
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshRenderer))]
	public class Water2DMaterialScaler : MonoBehaviour
	{
		public string SortingLayerName = "Water";

		public bool DisableInGame = true;
		public bool UpdateEachFrame = true;

		[Range(0f, 1f)] public float Transparency = 0.5f;
		[Range(0f, 1f)] public float RefractionIntensity = 0.02f;
		public float BumpMapTilling = 0.1f;
		public float TextureTilling = 1f;
		public float WaveDensity = 1f;
		public float WaveAmplitude = 0.2f;
		public float WaveSpeed = 0.5f;
		[Range(0.001f, 1f)] public float WaveEdgeSoftness = 0.7f;
		[Range(0f, 1f)] public float WaveBlendLevel = 0.5f;
		
		public float CurrentSpeed = -0.15f;

		MeshRenderer _renderer;
		
		void Awake()
		{
			_renderer = GetComponent<MeshRenderer>();
			_renderer.sortingLayerName = SortingLayerName;

			if (Application.isPlaying)
			{
				if (DisableInGame)
					enabled = false;
				else
					Debug.LogWarning("Material is updated each frame (check DisableInGame to increase performance)");
			}
		}

	}
}
