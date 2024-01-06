using System.IO;
using _Project.Scripts.Systems;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// Used for presenting an instruction panel to the participant.
    /// </summary>
    public class InstructionsPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI instructionsText;

        private const string DefaultInstructions = 
            "For this session, you are tasked with playing a simple left-right-left beat on the drums in-front of you.\n" 
            + "You will also see a prompt indicating which drum you should strike in sync with the music";

        private bool areInstructionsShown = true;
        private string instPath;

        private void Awake() => GetInstructionsFromFile();


        private void OnEnable()
        {
            EventManager.MusicStartEvent += HideInstructions;
            EventManager.MusicResetEvent += ShowInstructions;
        }

        private void OnDisable()
        {
            EventManager.MusicStartEvent -= HideInstructions;
            EventManager.MusicResetEvent -= ShowInstructions;
        }

        private void ShowInstructions()
        {
            areInstructionsShown = true;
            instructionsText.transform.parent.gameObject.SetActive(areInstructionsShown);
        }
        private void HideInstructions()
        {
            areInstructionsShown = false;
            instructionsText.transform.parent.gameObject.SetActive(areInstructionsShown);
        }

        /// <summary>
        /// Fetch instructions from a text-file located in the root directory of the project
        /// </summary>
        /// <param name="filename">name of the text-file with extension type (i.e. instructions.txt)</param>
        private void GetInstructionsFromFile(string filename = "instructions.txt")
        {
            instPath = Path.Combine(Application.dataPath, filename);
            
            var file = File.Open(instPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader reader = new StreamReader(file);
            var text = reader.ReadToEnd();
            reader.Close();

            if (text.Length < 1)
            {
                using (StreamWriter outputFile = new StreamWriter(instPath))
                {
                    foreach (var line in DefaultInstructions.Split('\n'))
                        outputFile.WriteLine(line);
                }
            }
            else
            {
                instructionsText.text = text;
            }
        }
    }
}
