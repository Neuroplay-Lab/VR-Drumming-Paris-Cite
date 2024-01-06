using System;
using System.IO;
using DrumRhythmGame.Data;
using UnityEngine;

namespace _Project.Scripts.Systems
{
    /// <summary>
    /// Used for the formatting of the save data
    /// </summary>
    public class SaveData
    {
        private static readonly Lazy<SaveData> Lazy = new Lazy<SaveData>(() => new SaveData());
        public static SaveData Instance => Lazy.Value;

        private string _saveDir;
        private string SaveDir
        {
            get
            {
                if (string.IsNullOrEmpty(_saveDir))
                {
                    _saveDir = $@"{Application.dataPath}/SaveData";
                }
                
                if (!Directory.Exists(_saveDir))
                {
                    // Create directory when it doesn't exist
                    Directory.CreateDirectory(_saveDir);
                }

                return _saveDir;
            }
        }
        
        public readonly PreferenceData preferenceData;
        public readonly PartnerErrorData partnerErrorData;
        public readonly AvatarData avatarData;
        public readonly SceneryData sceneryData;

        /// <summary>
        /// Load all save data
        /// </summary>
        public SaveData()
        {
            preferenceData = Load<PreferenceData>(nameof(PreferenceData));
            partnerErrorData = Load<PartnerErrorData>(nameof(PartnerErrorData));
            avatarData = Load<AvatarData>(nameof(AvatarData));
            sceneryData = Load<SceneryData>(nameof(SceneryData));

            Application.quitting += SaveAll;
        }

        /// <summary>
        /// Load selected save data.
        /// If there is no file that has name of given class, the file will be automatically created.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T Load<T>(string fileName) where T : new()
        {
            var path = $"{SaveDir}/{fileName}.json";
            if (!File.Exists(path))
            {
                Save(new T(), fileName);
            }
            
            var json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// Save all save data
        /// </summary>
        private void SaveAll()
        {
            Save(preferenceData, nameof(PreferenceData));
            Save(partnerErrorData, nameof(PartnerErrorData));
            Save(avatarData, nameof(AvatarData));
            Save(sceneryData, nameof(sceneryData));
            Debug.Log($"Data saved in: {_saveDir}");
        }

        /// <summary>
        /// Save selected current save data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <typeparam name="T"></typeparam>
        private void Save<T>(T data, string fileName)
        {
            var json = JsonUtility.ToJson(data, true);
            // TODO: Add encryption
            File.WriteAllText($"{SaveDir}/{fileName}.json", json);
        }
    }
}