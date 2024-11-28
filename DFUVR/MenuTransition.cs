using BepInEx;
using DaggerfallWorkshop.AudioSynthesis.Synthesis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DFUVR
{
    public class ButtonHandler:MonoBehaviour
    {
        public static void MenuTransition(GameObject currentMenu, GameObject nextMenu, bool mainMenuC, bool mainMenuM,ref string value)
        {
            
            if (value != null){
                value = Var.calibrationInt.ToString();
            }
            Var.calibrationInt = 0;
            currentMenu.SetActive(false);
            nextMenu.SetActive(true);

            if (mainMenuC) 
            {
                GameObject.Find("CSetupToggle").GetComponent<Toggle>().isOn=true;
            }

        }
        public static void MenuTransition(GameObject currentMenu, GameObject nextMenu, bool mainMenuC, bool mainMenuM, ref string value1, ref string value2)
        {
            value1 = Var.calibrationInt.ToString();
            value2 = (Var.calibrationInt+1).ToString();


            Var.calibrationInt = 0;
            currentMenu.SetActive(false);
            nextMenu.SetActive(true);

            if (mainMenuC)
            {
                GameObject.Find("CSetupToggle").GetComponent<Toggle>().isOn = true;
            }

        }

        public static void Done()
        {
            Var.SaveAxis();
            Plugin.LoggerInstance.LogInfo(Var.lThumbStickVertical== "Axis1");
            Var.fStartMenu = false;
            string filePath = Path.Combine(Paths.PluginPath, "Settings.txt");
            string[] lines = File.ReadAllLines(filePath);
            lines[5] = "false";
            File.WriteAllLines(filePath, lines);
            Application.Quit();
        }
        public static void nextInt()
        {
            Var.calibrationInt++;
        }
        public static void prevInt() 
        { 
            Var.calibrationInt--;
        }
        
    }
}
