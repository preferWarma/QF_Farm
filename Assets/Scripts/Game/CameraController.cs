using DG.Tweening;
using UnityEngine;
using QFramework;

namespace Game
{
	public partial class CameraController : ViewController
	{
		[Tooltip("镜头平滑度")] public float smooth = 10f;
		private Transform _playerTransform;

		private static CameraController _instance;
		private static Camera _camera;
		public static bool CanMove { get; private set; } = true;
		
		private void Start()
		{
			_playerTransform = Global.Player.transform;
			_instance = this;
			_camera = GetComponent<Camera>();
		}

		private void Update()
		{
			if (!CanMove) return;
			
			var playerPos = _playerTransform.position;
			var cameraPos = transform.position;
			var targetPos = new Vector3(playerPos.x, playerPos.y, cameraPos.z);	// 保持镜头的z轴不变
			transform.position = Vector3.Lerp(cameraPos, targetPos, 1 - Mathf.Exp(-Time.deltaTime * smooth));
		}
		
		public static void Shake(ShakeType shakeType)
		{
			CanMove = false;
			var strength = shakeType switch
			{
				ShakeType.Heavy => 0.2f,
				ShakeType.Middle => 0.05f,
				ShakeType.Light => 0.02f,
				_ => 0f
			};

			// 参数分别为：震动时间，震动幅度，震动次数，震动角度，是否随机角度，是否把初始位置作为震动的一部分，震动的随机性(枚举)
			_camera.transform.DOShakePosition(0.2f, strength, 100, 180, false, true, ShakeRandomnessMode.Harmonic)
				.OnComplete(() => { CanMove = true;});
		}

		private void OnDestroy()
		{
			_playerTransform = null;
			_instance = null;
		}
	}

	public enum ShakeType
	{
		Heavy,
		Middle,
		Light
	}
}
