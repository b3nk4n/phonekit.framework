using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        /// <summary>
        /// Speaks a text which code is secured with a try-catch block.
        /// </summary>
        /// <remarks>
        /// Use this method to ensure that the synthesazion will not cause
        /// any exceptions, e.g. HRESULT: 0x80045508.
        /// </remarks>
        /// <param name="content">The content text to speak.</param>
        public async Task TrySpeakTextAsync(string content)
        {
            try
            {
                await _instance.Synthesizer.SpeakTextAsync(content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Speaking text failed with error: " + ex.Message);
            }
        }

        /// <summary>
        /// Speaks a text which code is secured with a try-catch block.
        /// </summary>
        /// <remarks>
        /// Use this method to ensure that the synthesazion will not cause
        /// any exceptions, e.g. HRESULT: 0x80045508.
        /// </remarks>
        /// <param name="content">The content text to speak.</param>
        /// <param name="userState">The optional parameter for the completed event.</param>
        public async Task TrySpeakTextAsync(string content, object userState)
        {
            try
            {
                await _instance.Synthesizer.SpeakTextAsync(content, userState);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Speaking text failed with error: " + ex.Message);
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
