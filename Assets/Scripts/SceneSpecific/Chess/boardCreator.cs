/**
HandWaver, developed at the Maine IMRE Lab at the University of Maine's College of Education and Human Development
(C) University of Maine
See license info in readme.md.
www.imrelab.org
**/

﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

namespace IMRE.Chess3D
{
	/// <summary>
	/// Builds an array for  the spatial chess board.
	/// </summary>
	public class boardCreator : MonoBehaviour
	{

		public Material mat;

		void Awake()
		{
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 10; j++)
				{
					for (int k = 0; k < 10; k++)
					{
						string name = "Sphere" + i + j + k;
						GenerateSphere(name, .5f * (new Vector3(i, j, k)));
					}
				}
			}

		}


		private GameObject GenerateSphere(string name, Vector3 spawnPoint)
		{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.name = name;
			sphere.transform.SetParent(transform);
			sphere.transform.position = spawnPoint;
			sphere.GetComponent<Renderer>().material = mat;
			sphere.transform.localScale = (0.015f * Vector3.one);
			sphere.AddComponent<SphereCollider>().isTrigger = true;

			return sphere;
		}
	}
}