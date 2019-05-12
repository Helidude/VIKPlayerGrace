using NLog;
using System;
using System.IO;
using System.Windows.Controls;
using Torch;
using Torch.API;
using Torch.API.Plugins;


namespace VIKPlayerGrace
{
    public class GracePlugin : TorchPluginBase, IWpfPlugin
    {
        private GraceControl _control;
        private Persistent<GraceConfig> _config;

        public static GracePlugin Instance { get; private set; }
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public GraceConfig Config => _config?.Data;

        /// <inheritdoc />
        public UserControl GetControl() => _control ?? (_control = new GraceControl(this));

        public void Save() => _config.Save();

        /// <inheritdoc />
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);
            var configFile = Path.Combine(StoragePath, "PlayerGrace.cfg");
            
            try
            {
                _config = Persistent<GraceConfig>.Load(configFile);
            }
            catch (Exception e)
            {
                Log.Warn(e);
            }

            if (_config?.Data == null)
                _config = new Persistent<GraceConfig>(configFile, new GraceConfig());

            var pgmr = new GraceManager(torch);
            torch.Managers.AddManager(pgmr);

            Instance = this;
        }
    }
}
