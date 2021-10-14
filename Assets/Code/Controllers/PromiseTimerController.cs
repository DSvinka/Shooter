using Code.Interfaces;
using RSG;

namespace Code.Controllers
{
    internal sealed class PromiseTimerController: IController, IExecute
    {
        private IPromiseTimer _promiseTimer;
        
        public PromiseTimerController(IPromiseTimer promiseTimer)
        {
            _promiseTimer = promiseTimer;
        }

        public void Execute(float deltaTime)
        {
            _promiseTimer.Update(deltaTime);
        }
    }
}