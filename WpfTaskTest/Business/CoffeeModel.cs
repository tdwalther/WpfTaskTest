using System;
using System.Collections.Generic;
using System.Text;

namespace WpfTaskTest.Business
{
    public partial class CoffeeModel
    {
        public bool WithMilk { get; set; }
        public bool WithSugar { get; set; }

        public override string ToString()
        {
            if( WithMilk && !WithSugar)
            {
                return "coffee black and sweet";
            }
            else if( !WithMilk && WithSugar)
            {
                return "coffee cream only";
            }
            else if( WithMilk && WithSugar)
            {
                return "coffee regular";
            }
            else
            {
                return "coffee black";
            }
        }
    }
}
