using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondsDelayWait : IEnumerator
{
	private float wait;
	private float _currentWait;

	/// <summary>Gets and Sets currentWait property.</summary>
	public float currentWait
	{
		get { return _currentWait; }
		private set { _currentWait = value; }
	}

	/// <summary>Gets current iterator's value.</summary>
	public object Current { get { return currentWait; } }

	/// <summary>SecondsDelayWait constructor.</summary>
	/// <param name="_wait">Delay's Wait.</param>
	public SecondsDelayWait(float _wait)
	{
		wait = _wait;
		currentWait = 0.0f;
	}

	/// <summary>Moves to the next iterator.</summary>
	/// <returns>True if it was able to move to next iterator.</returns>
	public bool MoveNext()
	{
		return ((currentWait += Time.deltaTime) < (wait + Mathf.Epsilon));
	}

	/// <summary>Resets Iterator.</summary>
	public void Reset()
	{
		currentWait = 0.0f;
	}

	/// <summary>Disposes Iterator.</summary>
	public void Dispose() {  }
}
