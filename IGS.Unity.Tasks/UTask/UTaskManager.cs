
namespace IGS.Unity.Tasks
{
    internal class UTaskManager : Singleton<UTaskManager>
    {
        internal static readonly UTaskRunner[] Runners = new UTaskRunner[]
        {
            new UTaskRunner(UTaskRunnerID.YieldUpdate),
            new UTaskRunner(UTaskRunnerID.Update),
            new UTaskRunner(UTaskRunnerID.LastUpdate)
        };

        internal static void AddTask(UTaskRunnerID runner, IUTaskCore task)
        {
            if(Instance != null)
            {
                Runners[(int)runner].Add(task);
            }
        }

        #region Unity Calls
        void OnDestroy()
        {
            Runners.ForEach(x => x.Clear());
        }

        void Update()
        {
            Runners.ForEach(x => x.Run());
        }
        #endregion

        protected override void OnAwake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
