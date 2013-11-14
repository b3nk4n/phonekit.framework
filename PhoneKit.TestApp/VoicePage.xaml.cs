using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhoneKit.Framework.Voice;
using Windows.Phone.Speech.Recognition;

namespace PhoneKit.TestApp
{
    public partial class VoicePage : PhoneApplicationPage
    {
        /// <summary>
        /// The speech engine.
        /// </summary>
        Speech _speech = Speech.Instance;

        public VoicePage()
        {
            InitializeComponent();
        }

        private async void Read_Click(object sender, RoutedEventArgs e)
        {
            string text = ReadText.Text;

            if (!string.IsNullOrEmpty(text))
                await _speech.Synthesizer.SpeakTextAsync(text);
        }

        private async void ListenUI_Click(object sender, RoutedEventArgs e)
        {
            var res = await _speech.RecognizerUI.RecognizeWithUIAsync();

            if (res.ResultStatus == SpeechRecognitionUIStatus.Succeeded)
            {
                ListenText.Text = res.RecognitionResult.Text;
            }
        }

        private async void Listen_Click(object sender, RoutedEventArgs e)
        {
            var text = string.Empty;
            try
            {
                var res = await _speech.Recognizer.RecognizeAsync();
                text = res.Text;
            }
            catch (Exception ex)
            {
                text = ex.Message;
            }

            ListenText.Text = text;
        }
    }
}