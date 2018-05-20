using System;
using UnityEngine;

public class LayerMaskUtils
{
	public static readonly int GROUND_LAYER_MASK = (1 << LayerMask.NameToLayer("Ground"));
	public static readonly int PLAYER_LAYER_MASK = (1 << LayerMask.NameToLayer ("Player"));
	public static readonly int PLAYER_AND_GROUND_LAYER_MASK = GROUND_LAYER_MASK | PLAYER_LAYER_MASK;
}

