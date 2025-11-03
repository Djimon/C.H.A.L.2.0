using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayatGames.SaveGameFree.Types
{
	
	/// <summary>
	/// Representation of 2D vectors and points.
	/// </summary>
	[Serializable]
	public struct Vector2Save
	{
	
		public float x;
		public float y;

		public Vector2Save ( float x )
		{
			this.x = x;
			this.y = 0f;
		}

		public Vector2Save ( float x, float y )
		{
			this.x = x;
			this.y = y;
		}

		public Vector2Save ( Vector2 vector )
		{
			this.x = vector.x;
			this.y = vector.y;
		}

		public Vector2Save ( Vector3 vector )
		{
			this.x = vector.x;
			this.y = vector.y;
		}

		public Vector2Save ( Vector4 vector )
		{
			this.x = vector.x;
			this.y = vector.y;
		}

		public Vector2Save ( Vector2Save vector )
		{
			this.x = vector.x;
			this.y = vector.y;
		}

		public Vector2Save ( Vector3Save vector )
		{
			this.x = vector.x;
			this.y = vector.y;
		}

		public Vector2Save ( Vector4Save vector )
		{
			this.x = vector.x;
			this.y = vector.y;
		}

/// <summary>
/// Converts a Vector2 to a Vector2Save implicitly.
/// </summary>
/// <param name="vector">The Vector2 instance to convert.</param>
/// <returns>A new Vector2Save instance.</returns>
		public static implicit operator Vector2Save ( Vector2 vector )
		{
			return new Vector2Save ( vector );
		}

/// <summary>
/// Converts a Vector2Save to a Vector2 implicitly.
/// </summary>
/// <param name="vector">The Vector2Save instance to convert.</param>
/// <returns>A new Vector2 instance.</returns>
		public static implicit operator Vector2 ( Vector2Save vector )
		{
			return new Vector2 ( vector.x, vector.y );
		}

/// <summary>
/// Converts a Vector3 to a Vector2Save implicitly.
/// </summary>
/// <param name="vector">The Vector3 instance to convert.</param>
/// <returns>A new Vector2Save instance.</returns>
		public static implicit operator Vector2Save ( Vector3 vector )
		{
			return new Vector2Save ( vector );
		}

/// <summary>
/// Converts a Vector2Save to a Vector3 implicitly.
/// </summary>
/// <param name="vector">The Vector2Save instance to convert.</param>
/// <returns>A new Vector3 instance.</returns>
		public static implicit operator Vector3 ( Vector2Save vector )
		{
			return new Vector3 ( vector.x, vector.y );
		}

/// <summary>
/// Converts a Vector4 to a Vector2Save implicitly.
/// </summary>
/// <param name="vector">The Vector4 instance to convert.</param>
/// <returns>A new Vector2Save instance.</returns>
		public static implicit operator Vector2Save ( Vector4 vector )
		{
			return new Vector2Save ( vector );
		}

/// <summary>
/// Converts a Vector2Save to a Vector4 implicitly.
/// </summary>
/// <param name="vector">The Vector2Save instance to convert.</param>
/// <returns>A new Vector4 instance.</returns>
		public static implicit operator Vector4 ( Vector2Save vector )
		{
			return new Vector4 ( vector.x, vector.y );
		}

/// <summary>
/// Converts a Vector3Save to a Vector2Save implicitly.
/// </summary>
/// <param name="vector">The Vector3Save instance to convert.</param>
/// <returns>A new Vector2Save instance.</returns>
		public static implicit operator Vector2Save ( Vector3Save vector )
		{
			return new Vector2Save ( vector );
		}

/// <summary>
/// Converts a Vector2Save to a Vector3Save implicitly.
/// </summary>
/// <param name="vector">The Vector2Save instance to convert.</param>
/// <returns>A new Vector3Save instance.</returns>
		public static implicit operator Vector3Save ( Vector2Save vector )
		{
			return new Vector3Save ( vector );
		}

/// <summary>
/// Converts a Vector4Save to a Vector2Save implicitly.
/// </summary>
/// <param name="vector">The Vector4Save instance to convert.</param>
/// <returns>A new Vector2Save instance.</returns>
		public static implicit operator Vector2Save ( Vector4Save vector )
		{
			return new Vector2Save ( vector );
		}

/// <summary>
/// Converts a Vector2Save to a Vector4Save implicitly.
/// </summary>
/// <param name="vector">The Vector2Save instance to convert.</param>
/// <returns>A new Vector4Save instance.</returns>
		public static implicit operator Vector4Save ( Vector2Save vector )
		{
			return new Vector4Save ( vector );
		}

	}

}
