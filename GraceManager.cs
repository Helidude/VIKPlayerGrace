using NLog;
using System.Threading.Tasks;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Session;
using Torch.Managers;
using Torch.Managers.PatchManager;
using Torch.Session;

namespace VIKPlayerGrace
{
    class GraceManager : Manager
    {
        [Dependency] private readonly PatchManager _patchManager;
        private PatchContext _ctx;
        private TorchSessionManager _sessionManager;

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public GraceManager(ITorchBase torchInstance) : base(torchInstance)
        {
        }

        public override void Attach()
        {
            base.Attach();

            _sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            if (_sessionManager != null)
                _sessionManager.SessionStateChanged += SessionChanged;
            else
                Log.Warn("No session manager loaded!");

            if (_ctx == null)
                _ctx = _patchManager.AcquireContext();
        }

        public override void Detach()
        {
            base.Detach();

          

            _patchManager.FreeContext(_ctx);
        }

        private void SessionChanged(ITorchSession session, TorchSessionState state)
        {
            switch (state)
            {
                case TorchSessionState.Loaded:
                    Task.Delay(3000).ContinueWith((t) =>
                    {
                        Log.Debug("Patching MyLargeTurretBasePatch");

                        _patchManager.Commit();
                    });
                    break;
            }
        }
    }
}