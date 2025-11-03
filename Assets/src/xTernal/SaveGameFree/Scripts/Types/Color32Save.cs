using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayatGames.SaveGameFree.Types
{

	/// <summary>
	/// Representation of RGBA color in 32 bit format.
	/// </summary>
	[Serializable]
	public struct Color32Save
	{

		public byte r;
		public byte g;
		public byte b;
		public byte a;

		public Color32Save ( Color32 color )
		{
			this.r = color.r;
			this.g = color.g;
			this.b = color.b;
			this.a = color.a;
		}

/// <summary>
/// Converts a Color32 instance to a Color32Save instance.
/// </summary>
/// <param name="color">The Color32 instance to convert.</param>
/// <returns>A Color32Save representation of the given Color32.</returns>
		public static implicit operator Color32Save ( Color32 color )
		{
			return new Color32Save ( color );
		}

/// <summary>
/// Converts a Color32Save instance to a Color32 instance.
/// </summary>
/// <param name="color">The Color32Save instance to convert.</param>
/// <returns>A Color32 representation of the given Color32Save.</returns>
		public static implicit operator Color32 ( Color32Save color )
		{
			return new Color32 ( color.r, color.g, color.b, color.a );
		}

	}

}
