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
            _synthesizer = new SpeechSynthesizer();
            _synthesizer.SetVoice(InstalledVoices.Default);
            // TODO: check what happens if none of the app's supported languages is installed
            //       and how to implement a fallback to "en".

            try
            {
                // speech recognition support depends on the installed languages
                _recognizerUI = new SpeechRecognizerUI();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Instantiation of SpeechRecognizerFailed with error: " + ex.Message);
            }
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
        /// <remarks>
        /// Verify whether speech recognizer can be accessed using <code>HasRecognizerUI</code>
        /// </remarks>
        public SpeechRecognizerUI RecognizerUI
        {
            get
            {
                return _recognizerUI;
            }
        }

        /// <summary>
        /// Indicates whether the RecognizerUI was initialized successfully
        /// depending on the installed languages of the phone.
        /// </summary>
        public bool HasRecognizerUI
        {
            get
            {
                return _recognizerUI != null;
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
