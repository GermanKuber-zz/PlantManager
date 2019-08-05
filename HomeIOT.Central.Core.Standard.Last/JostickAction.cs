// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HomeIOT.Central.Core.Standard.Last
{
    public class JostickAction
    {
        public bool Used { get; set; } = false;
        public bool IsOn { get; set; } = false;
        public void On()
        {
            Used = true;
            IsOn = true;
        }
        public void Off()
        {
            Used = true;
            IsOn = false;
        }
        public void Change()
        {
            Used = true;
            IsOn = !IsOn;
        }
    }
}
