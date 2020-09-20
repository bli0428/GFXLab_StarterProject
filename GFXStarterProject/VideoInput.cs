using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GFXStarterProject
{
    /// <summary>
    /// A node to contain imported videos from files
    /// </summary>
    internal class VideoInput : Transform
    {
        public MediaPlayerElement UploadVideo { get; set; }

        public VideoInput(IStorageFile file)
        {
            // Initialize the video mediaPlayer and retrieve the video from file
            UploadVideo = new MediaPlayerElement();
            var mediaSource = MediaSource.CreateFromStorageFile(file);
            var mediaPlayer = new MediaPlayer {Source = mediaSource};
            UploadVideo.SetMediaPlayer(mediaPlayer);
            // Play the video
            mediaPlayer.Play();
            // Center the video in the user view
            UploadVideo.Margin = new Thickness(
                (OffsetX + ViewWidth / 2) / Zoom,
                (OffsetY + ViewHeight / 2) / Zoom,
                0, 0);

            // Enable touch, pinch, and drag event responses
            AllowTouch(UploadVideo);
            AllowResize(UploadVideo);
            AllowTranslation(UploadVideo);

            // Add the video node to connector's dictionary
            Connector.Scales.Add(UploadVideo, 1);
        }

        // Do nothing in this case
        protected override void ChangeColor(Color color)
        {
        }
    }
}
