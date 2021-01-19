using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System.IO;
using UnityEngine.Profiling;


namespace  evolis3d.SystemReplay
{
    public enum ReplayModeEnum
    {
        Recording,
        Playing
    }

    public class ReplayData : MonoBehaviour
    {
        #region memorystream
            private MemoryStream memoryStream = null;
            private BinaryWriter binaryWriter = null;
            private BinaryReader binaryReader = null;
        #endregion
        
        //private Dictionary<string, Transform> Actors; //WIP
        private List<Transform> Actors;
        
        #region deadzones y delay time
            private float _deadzoneOffset;
            public float DeadzoneOffset
            {
                get => _deadzoneOffset;
                set => _deadzoneOffset = value;
            }
        #endregion

        private ReplayModeEnum _replayMode;
        public ReplayModeEnum ReplayMode
        {
            get => _replayMode;
            set
            {
                _replayMode = value;

                if (value == ReplayModeEnum.Recording)
                {
                    InitRecording();

                }
                else if (value == ReplayModeEnum.Playing)
                {
                    InitReplaying();
                } 
            }
        }

        #region notifications & events
            public delegate void NotifyRecordingStarted();
            public delegate void NotifyRecordingStopped();
            public delegate void NotifyPlaybackStarted();
            public delegate void NotifyPlaybackStopped();
            public delegate void NotifyPlaybackPaused();
            public delegate void NotifyPlaybackResume();

            public event NotifyRecordingStarted RecordingStarted;
            public event NotifyRecordingStopped RecordingStopped;
            public event NotifyPlaybackStarted PlaybackStarted;
            public event NotifyPlaybackStopped PlaybackStopped;
            public event NotifyPlaybackPaused PlaybackPaused;
            public event NotifyPlaybackResume PlaybackResume;
        #endregion

        #region internal functions

        private void Awake()
        {
            //Actors = new Dictionary<string, Transform>();
            Actors = new List<Transform>();
            
            //no sé si ésto va aquí...
            memoryStream = new MemoryStream();
            binaryWriter = new BinaryWriter(memoryStream);
            binaryReader = new BinaryReader(memoryStream);
        }

        /// <summary>
        /// Setups memoryStream for recording, resets mem position to 0, at the beginning.
        /// </summary>
        private void InitRecording()
        {
            memoryStream.SetLength(0);
            memoryStream.Seek(0, SeekOrigin.Begin);
            binaryWriter.Seek(0, SeekOrigin.Begin);
            
            RecordingStarted?.Invoke();
            print("Recoding started at: " + Time.time);
        }

        /// <summary>
        /// TODO: Setups memoryStream for playback the recorded data.
        /// </summary>
        private void InitReplaying()
        {
            memoryStream.Seek(0, SeekOrigin.Begin);
            
            PlaybackStarted?.Invoke();
            print("Playback started at: " + Time.time);
        }
        
        /// <summary>
        /// Writes the transform on the memoryStream, I think...
        /// </summary>
        /// <param name="transform"></param>
        private void SaveTransform(string tagname, Transform transf)
        {
            binaryWriter.Write(tagname);
            binaryWriter.Write(transf.localPosition.x);
            binaryWriter.Write(transf.localPosition.y);
            binaryWriter.Write(transf.localPosition.z);
        }

        private void SaveTransform(Transform transf)
        {
            binaryWriter.Write(transf.localPosition.x);
            binaryWriter.Write(transf.localPosition.y);
            binaryWriter.Write(transf.localPosition.z);
        }

        /// <summary>
        /// Reads from the memoryStream and assigns to a Transform
        /// </summary>
        /// <param name="transf"></param>
        private void LoadTransform(Transform transf)
        {
           
            //aqui tiene que ir algun filtro o algo... WIP
            string tagname = binaryReader.ReadString();
            float x = binaryReader.ReadSingle();
            float y = binaryReader.ReadSingle();
            float z = binaryReader.ReadSingle();
            transf.localPosition = new Vector3(x, y, z);
        }
        
        /// <summary>
        /// AQUI VA LO GORDO!! Checks the current ReplayMode and records or playbacks to/from the memoryStream.
        /// </summary>
        private void FixedUpdate()
        {
            if (ReplayMode == ReplayModeEnum.Recording)
            {
                foreach (var item in Actors)
                {
                   //SaveTransform(item.Key, item.Value);
                   SaveTransform(item);
                }
            }
            else if(ReplayMode == ReplayModeEnum.Playing)
            {
                if (memoryStream.Position >= memoryStream.Length)
                {
                    StopReplaying();
                    //return;
                }
                else
                {
                    //asignar y reproducir cada uno...WIP
                    foreach (var item in Actors)
                    {
                        LoadTransform(item);
                    }
                    
                }
            }
        }

        private void StopReplaying()
        {
            //AQUI TODAVIA NO VA NADA, PERO DEBERIA IR ALGO!!!
            PlaybackStopped?.Invoke();
            print("Playback ended at: "+ Time.time);
        }


        /// <summary>
        /// Reduces the amount of stored data by checking the current min distance offset deadzone.
        /// </summary>
        private void TrimList()
        {
            Transform lastTransf;
            
            //TODO: limpiar items de la lista que tengan muy poca diferencia en valores.

            foreach (var item in Actors)
            {
                if (item == Actors[0]) continue;
                
                lastTransf = item;
            }
            print("Data trimmed succesfully."); 
        }
        
        
        
       
        #endregion
        
        

        #region API & public functions
            
            /// <summary>
            /// Returns the transform of the item, if it was stored in the list. Otherwise, returns null. 
            /// </summary>
            /// <param name="tagname"></param>
            /// <returns></returns>
            [CanBeNull]
            public Transform GetFromList(string tagname)
            {
                Transform temp;
                
                if (Actors.Count == 0) return null;
                
                //var isTemp = Actors.TryGetValue(tagname, out temp);
                temp = this.transform; //WIP I KNOW!!!
                return temp;
            }

      
            /// <summary>
            /// Stores a Transform to the internal list, for later use in the Replay System.
            /// You may want to store an item based on its Tag or GameObject's name...
            /// </summary>
            /// <param name="tagname"></param>
            /// <param name="transformData"></param>
            public void AddToList(string tagname, Transform transformData)
            {
                if (string.IsNullOrEmpty(tagname) || transformData == null) return;
                
                //Actors.Add(tagname,transformData);
                Actors.Add(transformData);
            }
            
        #endregion
       

        

    }
}
