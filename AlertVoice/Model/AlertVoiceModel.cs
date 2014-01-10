using System.ComponentModel;

namespace FFXIV.Tools.AlertVoice.Model
{
    public class AlertVoiceModel : INotifyPropertyChanged
    {
        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                if (value != _text)
                {
                    _text = value;
                    this.NotifyPropertyChanged("Text");
                }
            }
        }

        private string _debugText;
        public string DebugText
        {
            get
            {
                return _debugText;
            }

            set
            {
                if (value != _debugText)
                {
                    _debugText = value;
                    this.NotifyPropertyChanged("DebugText");
                }
            }
        }

        private bool _isDebugMode;
        public bool IsDebugMode
        {
            get
            {
                return _isDebugMode;
            }

            set
            {
                if (value != _isDebugMode)
                {
                    _isDebugMode = value;
                    this.NotifyPropertyChanged("IsDebugMode");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            if (null != this.PropertyChanged)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
