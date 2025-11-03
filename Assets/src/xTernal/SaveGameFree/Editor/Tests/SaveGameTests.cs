using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace BayatGames.SaveGameFree.Tests
{

/// <summary>
/// Contains tests for saving game data.
/// </summary>
	public class SaveGameTests
	{

		[Test]
/// <summary>
/// Saves the tests by validating input identifiers.
/// </summary>
		public void SaveTests ()
		{

			// Null identifier
			Assert.Catch ( () =>
			{
				SaveGame.Save<string> ( null, null );
			} );

			// Empty identifier
			Assert.Catch ( () =>
			{
				SaveGame.Save<string> ( "", null );
			} );

			// Simple save/load
			SaveGame.Save<string> ( "test/save", "saved" );
			Assert.IsTrue ( SaveGame.Exists ( "test/save" ) );
			Assert.AreEqual ( SaveGame.Load<string> ( "test/save", "not saved" ), "saved" );

			// Clear at end
			SaveGame.Clear ();
		}

		[Test]
/// <summary>
/// Loads the test cases for the SaveGame functionality.
/// This method verifies the behavior with null and empty identifiers.
/// </summary>
		public void LoadTests ()
		{

			// Null identifier
			Assert.Catch ( () =>
			{
				SaveGame.Load<string> ( null, "" );
			} );

			// Empty identifier
			Assert.Catch ( () =>
			{
				SaveGame.Load<string> ( "", "" );
			} );

			// Simple save/load
			SaveGame.Save<string> ( "test/load", "saved" );
			Assert.IsTrue ( SaveGame.Exists ( "test/load" ) );
			Assert.AreEqual ( SaveGame.Load<string> ( "test/load", "not saved" ), "saved" );

			// Reset to default
			Assert.IsFalse ( SaveGame.Exists ( "test/load2" ) );
			Assert.AreEqual ( SaveGame.Load<string> ( "test/load2", "not saved" ), "not saved" );

			// Clear at end
			SaveGame.Clear ();
		}

		[Test]
/// <summary>
/// Tests the Exists method of the SaveGame class for null and empty identifiers.
/// </summary>
		public void ExistsTests ()
		{

			// Null identifier
			Assert.Catch ( () =>
			{
				SaveGame.Exists ( null );
			} );

			// Empty identifier
			Assert.Catch ( () =>
			{
				SaveGame.Exists ( "" );
			} );

			// Check existent
			Assert.IsFalse ( SaveGame.Exists ( "test/exists" ) );
			SaveGame.Save<string> ( "test/exists", "saved" );
			Assert.IsTrue ( SaveGame.Exists ( "test/exists" ) );

			// Clear at end
			SaveGame.Clear ();
		}

		[Test]
/// <summary>
/// Deletes tests related to the SaveGame functionality.
/// This method verifies that deleting with null or empty identifiers throws exceptions.
/// </summary>
		public void DeleteTests ()
		{

			// Null identifier
			Assert.Catch ( () =>
			{
				SaveGame.Delete ( null );
			} );

			// Empty identifier
			Assert.Catch ( () =>
			{
				SaveGame.Delete ( "" );
			} );

			// Simple delete
			SaveGame.Save<string> ( "test/delete", "saved" );
			Assert.IsTrue ( SaveGame.Exists ( "test/delete" ) );
			SaveGame.Delete ( "test/delete" );
			Assert.IsFalse ( SaveGame.Exists ( "test/delete" ) );
			Assert.AreEqual ( SaveGame.Load<string> ( "test/delete", "not saved" ), "not saved" );

			// Clear at end
			SaveGame.Clear ();
		}

		[Test]
/// <summary>
/// Clears all test data from the save game.
/// </summary>
		public void ClearTests ()
		{
			
			// Clear all
			SaveGame.Save<string> ( "test/clear", "saved" );
			SaveGame.Clear ();
			Assert.IsFalse ( SaveGame.Exists ( "test/clear" ) );
			Assert.AreEqual ( SaveGame.Load<string> ( "test/clear", "not saved" ), "not saved" );
		}
		
	}

}
