using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using NAudio.CoreAudioApi;
using NAudio.MediaFoundation;
using NAudio.Wave;
using NAudio.Win8.Wave.WaveOutputs;
using SoundVisualizationLib.Universal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace DoubanFM.Universal.ViewModels
{
    public class MainPageViewModel:ViewModel,ISpectrumPlayer
    {
        private readonly int fftDataSize = (int)FFTDataSize.FFT2048;
        private bool isPlaying;
        private SampleAggregator sampleAggregator;
        private IWavePlayer player;
        private WaveStream activeStream;
        private IWaveIn recorder;
        private MemoryStream recordStream;
        private IRandomAccessStream selectedStream;


        public WaveStream ActiveStream
        {
            get { return activeStream; }
            protected set
            {
                if (value != activeStream)
                {
                    activeStream = value;
                    OnPropertyChanged(() => this.ActiveStream);
                }
            }
        }

        public MainPageViewModel()
        {
            LoadCommand = new DelegateCommand(Load);
            PlayCommand = new DelegateCommand(Play);
            PauseCommand = new DelegateCommand(Pause);
            StopCommand = new DelegateCommand(Stop);
            RecordCommand = new DelegateCommand(Record);
            StopRecordingCommand = new DelegateCommand(StopRecording);
            MediaFoundationApi.Startup();
        }

        

        private void Stop()
        {
            if (player != null)
            {
                player.Stop();
                isPlaying = false;
            }
        }

        private void Pause()
        {
            if (player != null)
            {
                player.Pause();
                isPlaying = false;
            }
        }

        private async void Play()
        {
            if (player == null)
            {
                // Exclusive mode - fails with a weird buffer alignment error
                player = new WasapiOutRT(AudioClientShareMode.Shared, 200);
                player.Init(CreateReader);

                player.PlaybackStopped += PlayerOnPlaybackStopped;
            }

            if (player.PlaybackState != PlaybackState.Playing)
            {
                //reader.Seek(0, SeekOrigin.Begin);
                player.Play();
                IsPlaying = true;
            }
        }
        private IWaveProvider CreateReader()
        {
            if (activeStream is RawSourceWaveStream)
            {
                activeStream.Position = 0;
                return activeStream;
            }
            activeStream = new MediaFoundationReaderRT(selectedStream);
            return activeStream;
        } 

        private void Record()
        {
            if (recorder == null)
            {
                recorder = new WasapiCaptureRT();
                recorder.RecordingStopped += RecorderOnRecordingStopped;
                recorder.DataAvailable += RecorderOnDataAvailable;               
            }

            if (activeStream != null)
            {
                activeStream.Dispose();
                activeStream = null;
            }
            
            recorder.StartRecording();

        }   

       

        private async void RecorderOnDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            if (activeStream == null)
            {
                recordStream = new MemoryStream();
                activeStream = new RawSourceWaveStream(recordStream, recorder.WaveFormat);                
            }      
     
            await recordStream.WriteAsync(waveInEventArgs.Buffer, 0, waveInEventArgs.BytesRecorded);                      
        }

        private void StopRecording()
        {
            if (recorder != null)
            {
                recorder.StopRecording();
            }
        }

        private void RecorderOnRecordingStopped(object sender, StoppedEventArgs stoppedEventArgs)
        {
        }


        private void PlayerOnPlaybackStopped(object sender, StoppedEventArgs stoppedEventArgs)
        {
            this.IsPlaying = false;
        }

        private async void Load()
        {
            if (player != null)
            {
                player.Dispose();
                player = null;
            }
            activeStream = null; // will be disposed by player

            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            picker.FileTypeFilter.Add("*");
            var file = await picker.PickSingleFileAsync();
            if (file == null) return;
            var stream = await file.OpenAsync(FileAccessMode.Read);
            if (stream == null) return;
            this.selectedStream = stream;
            sampleAggregator = new SampleAggregator(fftDataSize);

        }

        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand PlayCommand { get; private set; }
        public DelegateCommand PauseCommand { get; private set; }
        public DelegateCommand StopCommand { get; private set; }
        public DelegateCommand RecordCommand { get; private set; }
        public DelegateCommand StopRecordingCommand { get; private set; }

        public MediaElement MediaElement { get; set; }


        public bool GetFFTData(float[] fftDataBuffer)
        {
            sampleAggregator.GetFFTResults(fftDataBuffer);
            return IsPlaying;
        }

        public int GetFFTFrequencyIndex(int frequency)
        {
            double maxFrequency;
            if (ActiveStream != null)
                maxFrequency = ActiveStream.WaveFormat.SampleRate / 2.0d;
            else
                maxFrequency = 22050; // Assume a default 44.1 kHz sample rate.
            return (int)((frequency / maxFrequency) * (fftDataSize / 2));
        }

        public bool IsPlaying
        {
            get { return isPlaying; }
            set
            {
                if(value!=isPlaying)
                {
                    isPlaying = value;
                    OnPropertyChanged(() => this.IsPlaying);
                }
            }
        }
    }
}
