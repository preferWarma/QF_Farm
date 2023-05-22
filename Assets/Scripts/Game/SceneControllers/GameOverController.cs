using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.SceneControllers
{
	public partial class GameOverController : ViewController
	{
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				SceneManager.LoadScene("Scenes/GameStart");
			}
		}
	}
}
