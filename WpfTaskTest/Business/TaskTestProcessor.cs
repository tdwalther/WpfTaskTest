using RandomStandard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfTaskTest.Business
{
    public delegate void MessageDelegate(string msg);
    public delegate void ProgressDelegate(int prog);
    public delegate void CoffeeDelegate(bool light, bool sweet);
    public delegate void ToastDelegate(bool butter, bool jelly);

    public class TaskTestProcessor
    {
        public MessageDelegate Message { get; set; }
        public ProgressDelegate Progress { get; set; }
        public CoffeeDelegate CoffeeCondiments { get; set; }
        public ToastDelegate ToastCondiments { get; set; }

        public async Task CookBreakfast()
        {
            int progress = 0;
            Task<CoffeeModel> coffeeAdornTask=null;
            Task<ToastModel> toastAdornTask=null;

            Message?.Invoke("starting breakfast");
            var coffeeTask = MakeCoffeeAsync();
            var eggsTask = MakeEggsAsync();
            var baconTask = MakeBaconAsync();
            var toastTask = MakeToastAsync();

            var allTasks = new List<Task> { eggsTask, baconTask, toastTask, coffeeTask };

            while (allTasks.Any())
            {
                var finished = await Task.WhenAny(allTasks);
                if (finished == eggsTask)
                {
                    progress += 25;
                    Progress?.Invoke(progress);
                    Message?.Invoke("plating eggs");
                }
                else if(finished == coffeeTask)
                {
                    coffeeAdornTask = AdornCoffee(coffeeTask.Result);
                    allTasks.Add(coffeeAdornTask);
                }
                else if (finished == baconTask)
                {
                    progress += 25;
                    Progress?.Invoke(progress);
                    Message?.Invoke("plating bacon");
                }
                else if (finished == toastTask)
                {
                    toastAdornTask = AdornToast(toastTask.Result);
                    allTasks.Add(toastAdornTask);
                }
                else if (finished == coffeeAdornTask)
                {
                    progress += 25;
                    Progress?.Invoke(progress);
                    CoffeeCondiments?.Invoke(coffeeAdornTask.Result.WithMilk, coffeeAdornTask.Result.WithSugar);
                    Message?.Invoke("coffee is ready");
                }
                else if (finished == toastAdornTask)
                {
                    progress += 25;
                    Progress?.Invoke(progress);
                    ToastCondiments?.Invoke(toastAdornTask.Result.WithButter, toastAdornTask.Result.WithJelly);
                    Message?.Invoke("plating toast");
                }
                allTasks.Remove(finished);
            }

            Message?.Invoke("breakfast up!");
            Thread.Sleep(250);
            Progress?.Invoke(0);
        }

        private Task<CoffeeModel> MakeCoffeeAsync()
        {
            return Task<CoffeeModel>.Run(() =>
            {
                Message?.Invoke("coffee down");
                Thread.Sleep(RandomNumbers.GetInteger(1000));
                Message?.Invoke("coffee up!");
                return new CoffeeModel();
            });
        }

        private Task<CoffeeModel> AdornCoffee(CoffeeModel coffee)
        {
            return Task<CoffeeModel>.Run(() =>
            {
                Message?.Invoke("adorning coffee");
                Thread.Sleep(RandomNumbers.GetInteger(100));
                coffee.WithMilk = RandomNumbers.GetDouble() > 0.5;
                Thread.Sleep(RandomNumbers.GetInteger(100));
                coffee.WithSugar = RandomNumbers.GetDouble() > 0.5;
                Thread.Sleep(RandomNumbers.GetInteger(100));
                Message?.Invoke(coffee.ToString());
                return coffee;
            });
        }

        private Task MakeEggsAsync()
        {
            return Task.Run(() =>
            {
                Message?.Invoke("eggs down");
                Thread.Sleep(RandomNumbers.GetInteger(1000));
                Message?.Invoke("eggs up!");
            });
        }

        private Task MakeBaconAsync()
        {
            return Task.Run(() =>
            {
                Message?.Invoke("bacon down");
                Thread.Sleep(RandomNumbers.GetInteger(1000));
                Message?.Invoke("bacon up!");
            });
        }

        private Task<ToastModel> MakeToastAsync()
        {
            return Task.Run(() =>
            {
                Message?.Invoke("toast down");
                Thread.Sleep(RandomNumbers.GetInteger(1000));
                Message?.Invoke("toast up!");
                return new ToastModel();
            });
        }

        private Task<ToastModel> AdornToast(ToastModel toast)
        {
            return Task<ToastModel>.Run(() =>
            {
                Message?.Invoke("adorning toast");
                Thread.Sleep(RandomNumbers.GetInteger(100));
                toast.WithButter = RandomNumbers.GetDouble() > 0.5;
                Thread.Sleep(RandomNumbers.GetInteger(100));
                toast.WithJelly = RandomNumbers.GetDouble() > 0.5;
                Thread.Sleep(RandomNumbers.GetInteger(100));
                Message?.Invoke(toast.ToString());
                return toast;
            });
        }
    }
}
