using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BayatGames.SaveGameFree.Types;

namespace BayatGames.SaveGameFree.Examples
{

    [System.Serializable]
    public class StorageSG
    {
        public System.DateTime myDateTime;

        public StorageSG()
        {
            myDateTime = System.DateTime.UtcNow;
        }
    }

/// <summary>
/// Manages saving and loading game positions.
/// </summary>
    public class ExampleSavePosition : MonoBehaviour
    {
        private string _encodePassword;

        public Transform target;
        public bool loadOnStart = true;
        public string identifier = "exampleSavePosition.dat";

        void Start()
        {
            _encodePassword = "12345678910abcdef12345678910abcdef";
            SaveGame.EncodePassword = _encodePassword;
            SaveGame.Encode = true;
            SaveGame.Serializer = new SaveGameFree.Serializers.SaveGameBinarySerializer();
            StorageSG ssg = new StorageSG();
            SaveGame.Save<StorageSG>("pizza2", ssg);
            StorageSG ssgLoaded = SaveGame.Load<StorageSG>("pizza2");
            Debug.Log(ssgLoaded.myDateTime.ToLocalTime().ToString());
            if (loadOnStart)
            {
                Load();
            }
        }

        void Update()
        {
            Vector3 newPosition = target.position;
            newPosition.x += Input.GetAxis("Horizontal");
            newPosition.y += Input.GetAxis("Vertical");
            target.position = newPosition;
        }

        void OnApplicationQuit()
        {
            Save();
        }

/// <summary>
/// Saves the current game state.
/// </summary>
        public void Save()
        {
            SaveGame.Save<Vector3Save>(identifier, target.position, SerializerDropdown.Singleton.ActiveSerializer);
        }

/// <summary>
/// Loads the saved game state and updates the target position.
/// </summary>
        public void Load()
        {
            target.position = SaveGame.Load<Vector3Save>(
                identifier,
                Vector3.zero,
                SerializerDropdown.Singleton.ActiveSerializer);
        }

    }

}
