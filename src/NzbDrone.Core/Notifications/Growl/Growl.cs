using System.Collections.Generic;
using FluentValidation.Results;
using NzbDrone.Common.Extensions;

namespace NzbDrone.Core.Notifications.Growl
{
    public class Growl : NotificationBase<GrowlSettings>
    {
        private readonly IGrowlService _growlService;

        public override string Name => "Growl";


        public Growl(IGrowlService growlService)
        {
            _growlService = growlService;
        }

        public override string Link => "http://growl.info/";

        public override void OnGrab(GrabMessage grabMessage)
        {
            _growlService.SendNotification(MOVIE_GRABBED_TITLE, grabMessage.Message, "GRAB", Settings.Host, Settings.Port, Settings.Password);
        }

        public override void OnDownload(DownloadMessage message)
        {
            _growlService.SendNotification(MOVIE_DOWNLOADED_TITLE, message.Message, "DOWNLOAD", Settings.Host, Settings.Port, Settings.Password);
        }


        public override ValidationResult Test()
        {
            var failures = new List<ValidationFailure>();

            failures.AddIfNotNull(_growlService.Test(Settings));

            return new ValidationResult(failures);
        }
    }
}
