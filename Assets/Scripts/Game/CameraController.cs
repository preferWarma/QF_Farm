using UnityEngine;
using QFramework;

namespace Game
{
	public partial class CameraController : ViewController
	{
		[Tooltip("镜头平滑度")] public float smooth = 10f;
		private Transform _playerTransform;
		
		private void Start()
		{
			_playerTransform = Global.Player.transform;
		}

		private void Update()
		{
			var playerPos = _playerTransform.position;
			var cameraPos = transform.position;
			var targetPos = new Vector3(playerPos.x, playerPos.y, cameraPos.z);	// 保持镜头的z轴不变
			transform.position = Vector3.Lerp(cameraPos, targetPos, 1 - Mathf.Exp(-Time.deltaTime * smooth));
		}
	}
}
