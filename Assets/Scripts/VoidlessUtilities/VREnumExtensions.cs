using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoidlessUtilities.VR
{
public static class VREnumExtensions
{
#region BodyDetectionFlagOperations:
	/// <summary>Checks if BodyDetection enumerator contains flag.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Requested flag to check.</param>
	/// <returns>True if the BodyDetection enumerator contains flag.</returns>
	public static bool HasFlag(this BodyDetection _enum, BodyDetection _flag){ return ((_enum & _flag) == _flag); }
	
	/// <summary>Checks if BodyDetection enumerator contains flags.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Requested flags to check.</param>
	/// <returns>True if the BodyDetection enumerator contains all flags.</returns>
	public static bool HasFlags(this BodyDetection _enum, params BodyDetection[] _flags)
	{
		for(int i = 0; i < _flags.Length; i++)
		{
			if(!((_enum & _flags[i]) == _flags[i])) return false;
		}
	
		return true;
	}
	
	/// <summary>Adds Flag [if there is not] to BodyDetection enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to add to the BodyDetection enumerator.</param>
	public static void AddFlag(ref BodyDetection _enum, BodyDetection _flag){ if(!_enum.HasFlag(_flag)) _enum |= _flag; }
	
	/// <summary>Adds Flags [if there are not] to BodyDetection enumerator.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flags to add to the BodyDetection enumerator.</param>
	public static void AddFlags(ref BodyDetection _enum, params BodyDetection[] _flags){ for(int i = 0; i < _flags.Length; i++) if(!_enum.HasFlag(_flags[i])) _enum |= _flags[i]; }
	
	/// <summary>Removes flag from BodyDetection enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to remove from BodyDetection enumerator.</param>
	public static void RemoveFlag(ref BodyDetection _enum, BodyDetection _flag){ if(_enum.HasFlag(_flag)) _enum ^= _flag; }
	
	/// <summary>Removes flags from BodyDetection enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to remove from BodyDetection enumerator.</param>
	public static void RemoveFlags(ref BodyDetection _enum, params BodyDetection[] _flags){ for(int i = 0; i < _flags.Length; i++) if(_enum.HasFlag(_flags[i])) _enum ^= _flags[i]; }
	
	/// <summary>Toggles flag from BodyDetection enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flag operation.</param>
	/// <param name='_flag'>Flag to toggle from BodyDetection enumerator.</param>
	public static void ToggleFlag(ref BodyDetection _enum, BodyDetection _flag){ _enum ^= _flag; }
	
	/// <summary>Toggles flags from BodyDetection enumerator, if it has it.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	/// <param name='_flags'>Flags to toggle from BodyDetection enumerator.</param>
	public static void ToggleFlags(ref BodyDetection _enum, params BodyDetection[] _flags){ for(int i = 0; i < _flags.Length; i++) _enum ^= _flags[i]; }
	
	/// <summary>Removes all BodyDetection enumerator's flags, leaving all its bits to '0', and on its default value.</summary>
	/// <param name='_enum'>Enumerator to make flags operation.</param>
	public static void RemoveAllFlags(ref BodyDetection _enum){ _enum = (BodyDetection)0; }
#endregion
}
}