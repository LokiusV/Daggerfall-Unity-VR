using BepInEx;
using DaggerfallWorkshop.AudioSynthesis.Synthesis;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public static string cType;
        public static string refresh_rate;
        public static void MenuTransition(GameObject currentMenu, GameObject nextMenu, bool mainMenuC, bool mainMenuM)
        {
            currentMenu.SetActive(false);
            nextMenu.SetActive(true);

            if (mainMenuC) 
            {
                GameObject.Find("HSetupToggle").GetComponent<Toggle>().isOn=true;
                if (GameObject.Find("MSetupToggle").GetComponent<Toggle>().isOn)
                {
                    Var.fSpawnDoneButton.GetComponent<Button>().interactable = true;
                }
            }
            if (mainMenuM)
            {
                GameObject.Find("MSetupToggle").GetComponent<Toggle>().isOn = true;
                if (GameObject.Find("HSetupToggle").GetComponent<Toggle>().isOn)
                {
                    Var.fSpawnDoneButton.GetComponent<Button>().interactable = true;
                }
            }

        }
        //public static void MenuTransition(GameObject currentMenu, GameObject nextMenu, bool mainMenuC, bool mainMenuM, ref string value1, ref string value2)
        //{
        //    value1 = Var.calibrationInt.ToString();
        //    value2 = (Var.calibrationInt+1).ToString();


        //    Var.calibrationInt = 0;
        //    currentMenu.SetActive(false);
        //    nextMenu.SetActive(true);

        //    if (mainMenuC)
        //    {
        //        GameObject.Find("CSetupToggle").GetComponent<Toggle>().isOn = true;
        //    }

        //}

        public static void Done()
        {
            //Var.SaveAxis();
            //Plugin.LoggerInstance.LogInfo(Var.lThumbStickVertical== "Axis1");
            Var.fStartMenu = false;
            string filePath = Path.Combine(Paths.PluginPath, "Settings.txt");
            string[] lines = File.ReadAllLines(filePath);
            lines[5] = "false";
            File.WriteAllLines(filePath, lines);
            Application.Quit();
        }
        public static void SaveButtonClick(GameObject currentMenu, GameObject nextMenu, bool mainMenuC, bool mainMenuM)
        {
            try
            {
                cType = GameObject.Find("CLabel").GetComponent<Text>().text;
                refresh_rate = GameObject.Find("HzLabel").GetComponent<Text>().text;
                string filePath = Path.Combine(Paths.PluginPath, "Settings.txt");
                string[] lines = File.ReadAllLines(filePath);
                lines[2] = string.Format(CultureInfo.InvariantCulture, cType);
                lines[1] = string.Format(CultureInfo.InvariantCulture, refresh_rate);
                File.WriteAllLines(filePath, lines);
                MenuTransition(currentMenu, nextMenu, mainMenuC, mainMenuM);
            }
            catch(Exception e) { Plugin.LoggerInstance.LogError(e); }
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
