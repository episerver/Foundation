using EPiServer.PlugIn;
using EPiServer.Scheduler;

namespace Foundation.Commerce.Marketing
{
    [ScheduledPlugIn(DisplayName = "Delete expired coupon job.", GUID = "C9F62244-A3FF-4579-B68F-F7185BF6E669")]
    public class DeleteExpiredCouponJob : ScheduledJobBase
    {
        private bool _stopSignaled;
        private readonly IUniqueCouponService _couponService;

        public DeleteExpiredCouponJob(IUniqueCouponService couponService)
        {
            IsStoppable = true;
            _couponService = couponService;
        }

        public override void Stop() => _stopSignaled = true;

        public override string Execute()
        {
            OnStatusChanged(string.Format("Starting execution of {0}", GetType()));

            var result = _couponService.DeleteExpiredCoupons();

            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            if (result)
            {
                return "Job runs sucessfully";
            }
            else
            {
                return "There is problem with job execution";
            }
        }
    }
}
