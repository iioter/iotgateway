namespace WalkingTec.Mvvm.Core.WorkFlow
{
    public interface IApproveNotification
    {
        void OnStart(ApproveInfo info);

        void OnResume(ApproveInfo info);
    }
}