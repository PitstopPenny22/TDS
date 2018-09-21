using System.ComponentModel;

namespace ViewModelUtils.Enums
{
    public enum Axle
    {
        Front,
        Rear
    }
    public enum TyreFamily
    {
        NotSet,
        F1,
        F2
    }
    public enum TyrePlacement
    {
        NotSet,
        [Description("Front Left")]
        [Axle(Axle.Front)]
        FL,
        [Description("Front Right")]
        [Axle(Axle.Front)]
        FR,
        [Description("Rear Left")]
        [Axle(Axle.Rear)]
        RL,
        [Description("Rear Right")]
        [Axle(Axle.Rear)]
        RR
    }
    public enum TyreType
    {
        NotSet,
        [Percentage(80)]
        SuperSoft,
        [Percentage(80)]
        Soft,
        [Percentage(90)]
        Medium,
        [Percentage(75)]
        Hard
    }
}
