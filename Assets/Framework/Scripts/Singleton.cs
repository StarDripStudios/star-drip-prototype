using System;
using System.Collections;
using UnityEngine;

namespace Framework
{
	public abstract class Singleton<T> : MonoBehaviour, IInitializable where T : MonoBehaviour
	{
		public static bool HasInstance => _instance != null;

		private static object _lock = new object();
		private static bool _isQuitting;

		#region Singleton

		private static T _instance;

		public static T Instance
		{
			get
			{
				lock (_lock)
				{
					return _instance;
				}
			}

			private set
			{
				if (_instance != null || _isQuitting)
				{
					return;
				}

				lock (_lock)
				{
					_instance = value;

					var instanceTransform = _instance.transform;
					instanceTransform.SetParent(null);
					instanceTransform.position = Vector3.zero;
					instanceTransform.rotation = Quaternion.identity;
					instanceTransform.localScale = Vector3.one;

					var instanceObject = _instance.gameObject;
					instanceObject.name = typeof(T).Name;
					DontDestroyOnLoad(instanceObject);
				}
			}
		}

		protected virtual void Awake()
		{
			if (_instance == null)
			{
				Instance = this as T;
			}
			else if (_instance != this)
			{
				Destroy(gameObject);
			}
		}

		protected virtual void OnDestroy()
		{
			_isQuitting = true;
		}

		#endregion Singleton

		#region IInitializable

		/// <summary>
		/// Invokes the action when the manager is initialized.
		/// </summary>
		public static void InvokeAfterInitialization(Action eventHandler)
		{
			if (HasInstance && Instance is IInitializable initializable)
			{
				if (initializable.IsInitialized)
				{
					eventHandler();
					return;
				}
			}

			InitializedEvent -= eventHandler;
			InitializedEvent += eventHandler;
		}

		public static event Action InitializedEvent;

		public virtual bool IsInitialized { get; protected set; }
		public virtual bool IsInitializing { get; protected set; }

		public virtual void Initialize()
		{
			// Early out if we are already initializing.
			if (IsInitialized || IsInitializing) return;

			// Early out if our singleton object is being destroyed.
			if (null == this || null == gameObject) return;

			StartCoroutine(InitializeRoutine());
		}

		protected virtual IEnumerator InitializeRoutine()
		{
			IsInitializing = true;
			
			yield return StartCoroutine(WaitForDependencies());
			
			IsInitializing = false;
			IsInitialized = true;
			
			InitializedEvent?.Invoke();
		}

		#endregion IInitializable

		protected virtual IEnumerator WaitForDependencies()
		{
			yield break;
		}
	}
}
