using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BayatGames.SaveGameFree.Types;

namespace BayatGames.SaveGameFree.Examples
{

/// <summary>
/// Manages saving and loading of rotation data in a Unity game.
/// </summary>
	public class ExampleSaveRotation : MonoBehaviour
	{

		public Transform target;
		public bool loadOnStart = true;
		public string identifier = "exampleSaveRotation.dat";

		void Start ()
		{
			if ( loadOnStart )
			{
				Load ();
			}
		}

		void Update ()
		{
			Vector3 rotation = target.rotation.eulerAngles;
			rotation.z += Input.GetAxis ( "Horizontal" );
			target.rotation = Quaternion.Euler ( rotation );
		}

		void OnApplicationQuit ()
		{
			Save ();
		}

/// <summary>
/// Saves the current state of the target object.
/// </summary>
		public void Save ()
		{
			SaveGame.Save<QuaternionSave> ( identifier, target.rotation, SerializerDropdown.Singleton.ActiveSerializer );
		}

/// <summary>
/// Loads the saved game state for the target object.
/// </summary>
		public void Load ()
		{
			target.rotation = SaveGame.Load<QuaternionSave> (
				identifier,
				Quaternion.identity,
				SerializerDropdown.Singleton.ActiveSerializer );
		}

	}

}
