using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class UIShowOrHideController : MonoBehaviour
    {
        [Tooltip("控制显示或隐藏的对象")] public GameObject obj;

        private void Start()
        {
            obj.SetActive(false);
            obj.transform.localScale = Vector3.zero;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                DoCloseOrOpen(obj, true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                DoCloseOrOpen(obj,false);
        }

        /// <summary>
        /// 使用DoTween实现界面的打开和关闭动画
        /// </summary>
        /// <param name="obj">显示或隐藏的对象</param>
        /// <param name="isOpen">打开(true),关闭(false)</param>
        public static void DoCloseOrOpen(GameObject obj, bool isOpen)
        {
            if (isOpen)
            {
                obj.transform.DOScale(Vector3.one, 0.3f)
                    .SetEase(Ease.OutCubic)
                    .OnStart(() => obj.SetActive(true))
                    .startValue = Vector3.zero;
            }
            else
            {
                obj.transform.DOScale(Vector3.zero, 0.3f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() => obj.SetActive(false))
                    .startValue = Vector3.one;
            }
        }
    }
}