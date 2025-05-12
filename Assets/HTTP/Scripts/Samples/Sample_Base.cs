using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace HTTP
{
    public abstract class Sample_Base :  MonoBehaviour
    {
        [SerializeField] protected TMP_Text requestTextUI;
        [SerializeField] protected TMP_Text responseTextUI;

        public void SendRequest()
        {
            StartCoroutine(RequestProcess());
        }

        protected abstract IEnumerator RequestProcess();
    }
}