using UnityEngine;
using QFramework;

namespace Game
{
	public partial class Home : ViewController
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			Debug.Log("进入家");
			if (other.name.StartsWith("Player"))
			{
				ActionKit.Delay(0.5f, () =>
				{
					Global.Days.Value++;
					other.transform.position = new Vector3();
				}).Start(this);
				
			}
		}
	}
}
