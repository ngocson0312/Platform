using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDEV.PlatformGame
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField] private Text m_titleText;
        [SerializeField] private Text m_contentText;

        public virtual void Show(bool isShow)
        {
            gameObject.SetActive(isShow);
        }

        public virtual void UpdateDialog(string title, string content)
        {
            if (m_titleText)
                m_titleText.text = title;

            if (m_contentText)
                m_contentText.text = content;
        }

        public virtual void UpdateDialog()
        {

        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
