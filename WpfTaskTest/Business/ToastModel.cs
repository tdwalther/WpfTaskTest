namespace WpfTaskTest.Business
{
    public class ToastModel
    {
        public bool WithButter { get; set; }
        public bool WithJelly { get; set; }


        public override string ToString()
        {
            if (WithButter && !WithJelly)
            {
                return "toast with butter";
            }
            else if (!WithButter && WithJelly)
            {
                return "toast with jelly";
            }
            else if (WithButter && WithJelly)
            {
                return "toast with butter and jelly";
            }
            else
            {
                return "plain 'ole toast";
            }
        }
    }
}
