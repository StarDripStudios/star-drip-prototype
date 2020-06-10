using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarDust
{
	public class LevelHeadFollower : MonoBehaviour
	{
		public Transform followTransform;
		public float distance;
		public float height;
		public float distanceCheck;

		private Vector3 _followPosition;

		private void LateUpdate()
		{
			_followPosition = followTransform.position;
			_followPosition.z += distance;
			_followPosition.y += height;

			transform.position = _followPosition;
		}

		private void OnDrawGizmos()
		{
			if (!followTransform) return;
			
			Gizmos.color = Color.blue;
			var position = followTransform.position;
			position.z += distance;
			position.y += height;
			Gizmos.DrawSphere(position, 0.1f);
		}
	}
}
