using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Supercargo;

[RequireComponent(typeof(PatternRecognizer))]
public class TESTCommandListener : MonoBehaviour
{
	[SerializeField] private Transform user; 	/// <summary>User.</summary>
	private Command currentCommand;
	private PatternRecognizer _patternRecognizer;

	/// <summary>Gets and Sets patternRecognizer Component.</summary>
	public PatternRecognizer patternRecognizer
	{ 
		get
		{
			if(_patternRecognizer == null)
			{
				_patternRecognizer = GetComponent<PatternRecognizer>();
			}
			return _patternRecognizer;
		}
	}

	void OnEnable()
	{
		PatternRecognizer.onPatternRecognized += FollowCommand;
	}

	void OnDisable()
	{
		PatternRecognizer.onPatternRecognized -= FollowCommand;
	}

	void Awake()
	{
		currentCommand = Command.Stop;
	}

	void Update()
	{
		Vector3 direction = (user.position - transform.position);
		//direction.y = 0.0f;

		switch(currentCommand)
		{
			case Command.MoveAhead:
			transform.position += direction.normalized * Time.deltaTime; 
			break;

			case Command.TurnLeft:
			case Command.TurnRight:
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), 0.5f);
			break;

			case Command.Stop:
			break;

			default:
			break;
		}
	}

	private void FollowCommand(Command _command)
	{
		if(_command != Command.None)
		currentCommand = _command;
		//else currentCommand = Command.Stop;
	}
}
