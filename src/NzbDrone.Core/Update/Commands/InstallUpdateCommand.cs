using NzbDrone.Core.Messaging.Commands;

namespace NzbDrone.Core.Update.Commands
{
    public class InstallUpdateCommand : Command
    {
        public override bool SendUpdatesToClient => true;
        public override bool IsExclusive => true;

        public override string CompletionMessage => null;

        public InstallUpdateCommand(UpdatePackage latestUpdate)
        {
            LatestUpdate = latestUpdate;
        }

        public UpdatePackage LatestUpdate { get; set; }
    }
}
