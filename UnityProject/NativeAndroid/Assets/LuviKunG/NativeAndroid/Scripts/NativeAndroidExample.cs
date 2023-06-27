using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuviKunG.Android;

namespace LuviKunG.Android.Examples
{
    public class NativeAndroidExample : MonoBehaviour
    {
        [SerializeField] private Button btnShowToast;

        private void Awake()
        {
            if (btnShowToast)
            {
                btnShowToast.onClick.AddListener(() =>
                {
                    NativeAndroid.ShowToastMessage("Hello, World");
                });
            }
        }
    }
}
