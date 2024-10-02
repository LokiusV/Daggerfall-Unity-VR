using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DFUVR
{
    public class KeyboardController : MonoBehaviour
    {
        void Start()
        {
            Button[] buttons = Var.keyboard.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {

                button.onClick.AddListener(() =>
                {

                    Var.keyboard.GetComponent<KeyboardController>().Add(button.name);
                });
            }
        }

        public void Add(string x)
        {
            System.Windows.Forms.Keys keyValue;
            if (Enum.TryParse(x, true, out keyValue))
            {
                KeySender.PressKey(keyValue, false);
                KeySender.PressKey(keyValue, true);
            }
            else
            {
                Plugin.LoggerInstance.LogInfo("Not A valid key: "+keyValue);
            }


        }
    }
}
