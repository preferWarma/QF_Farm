using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.SceneControllers
{
	public partial class GameStartController : ViewController
	{
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Global.LoadDefaultData();	// 新的游戏开始时，加载默认数据
				SceneManager.LoadScene("Scenes/Game");
			}
		}
	}
}
