using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BayatGames.SaveGameFree.Types
{

	/// <summary>
	/// Representation of RGBA color.
	/// </summary>
	[Serializable]
	public struct ColorSave
	{

		public float r;
		public float g;
		public float b;
		public float a;

		public ColorSave ( Color color )
		{
			this.r = color.r;
			this.g = color.g;
			this.b = color.b;
			this.a = color.a;
		}

/// <summary>
/// Converts a Color instance to a ColorSave instance.
/// </summary>
/// <param name="color">The Color instance to convert.</param>
/// <returns>A ColorSave instance representing the Color values.</returns>
		public static implicit operator ColorSave ( Color color )
		{
			return new ColorSave ( color );
		}

/// <summary>
/// Converts a ColorSave instance to a Color instance.
/// </summary>
/// <param name="color">The ColorSave instance to convert.</param>
/// <returns>A Color instance representing the ColorSave values.</returns>
		public static implicit operator Color ( ColorSave color )
		{
			return new Color ( color.r, color.g, color.b, color.a );
		}

	}

}
