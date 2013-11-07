using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Phone.Speech.Recognition;
using Windows.Phone.Speech.Synthesis;
using Windows.Phone.Speech.VoiceCommands;

namespace PhoneKit.Framework.Voice
{
    /// <summary>
    /// Bundles the speech recognition and synthesization system of the phone.
    /// </summary>
    public class Speech
    {
        #region Members

        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static Speech _instance;

        /// <summary>
        /// The voice-to-text module.
        /// </summary>
        private SpeechRecognizer _recognizer;

        /// <summary>
        /// The text-to-voice module.
        /// </summary>
        private SpeechSynthesizer _synthesizer;

        /// <summary>
        /// The voice-to-text user interface.
        /// </summary>
        private SpeechRecognizerUI _recognizerUI;

        /// <summary>
        /// Indicates whether voice commands have been installed.
        /// </summary>
        private bool _hasInstalledCommands = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Speech instance.
        /// </summary>
        private Speech()
        {
            Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Installs void commands from a Command Definition (VCD) XML file.
        /// </summary>
        /// <param name="voiceCommandSetsUri">The path to the voice commands file.</param>
        public async void InstallCommandSets(Uri voiceCommandSetsUri)
        {
            if (!_hasInstalledCommands)
            {
                try
                {
                    await VoiceCommandService.InstallCommandSetsFromFileAsync(voiceCommandSetsUri);

                    _hasInstalledCommands = true;
                }
                catch (Exception error)
                {
                    Debug.WriteLine("Voice Commands failed to install with error: " + error.Message);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the speech services.
        /// </summary>
        private void Initialize()
        {
            _recognizer = new SpeechRecognizer();

            _synthesizer = new SpeechSynthesizer();
            _synthesizer.SetVoice(InstalledVoices.Default);
            // TODO: check what happens if none of the app's supported languages is installed
            //       and how to implement a fallback to "en".

            _recognizerUI = new SpeechRecognizerUI();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the sound effects singleton instance.
        /// </summary>
        public static Speech Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Speech();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Gets the speech recognizer.
        /// </summary>
        public SpeechRecognizer Recognizer
        {
            get
            {
                return _recognizer;
            }
        }

        /// <summary>
        /// Gets the speech synthesizer.
        /// </summary>
        public SpeechSynthesizer Synthesizer
        {
            get
            {
                return _synthesizer;
            }
        }

        /// <summary>
        /// Gets the speech recognizer UI.
        /// </summary>
        public SpeechRecognizerUI RecognizerUI
        {
            get
            {
                return _recognizerUI;
            }
        }

        /// <summary>
        /// Gets whether voice commands have been installed.
        /// </summary>
        public bool HasInstalledCommands
        {
            get
            {
                return _hasInstalledCommands;
            }
        }

        #endregion
    }
}
