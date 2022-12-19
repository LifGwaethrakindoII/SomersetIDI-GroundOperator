using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFiniteStateMachine<T>
{
	T state { get; set; } 			/// <summary>Agent's Current State.</summary>
	T previousState { get; set; } 	/// <summary>Agent's Previous State.</summary>

	/// <summary>Enters State.</summary>	
	/// <param name="_state">State Entered.</param>
	void OnEnter(T _state);

	/// <summary>Exits State.</summary>
	/// <param name="_state">State Left.</param>
	void OnExit(T _state);
}
