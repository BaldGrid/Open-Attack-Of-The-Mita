    using MidiPlayerTK;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class DancingWalls : MonoBehaviour
    {
        public MidiFilePlayer midiPlayer;
        public VertexGlitchManager VGM;

        void Start()
        {
            if (midiPlayer != null)
                midiPlayer.OnEventNotesMidi.AddListener(OnNoteEvent);
        }

        void OnNoteEvent(List<MPTKEvent> notes)
        {
            float tempoFactor = 1f;

            foreach (MPTKEvent note in notes)
            {
                if (note.Channel == 9) // MIDI Channel 10 (drums)
                {
                    int drumNote = note.Value;

                    if (drumNote == 38 || drumNote == 79 || drumNote == 78 || drumNote == 46)
                    {
                    //VGM.Glitch(tempoFactor);
                        VGM.Glitch();
                    }
                }
            }
        }
    }