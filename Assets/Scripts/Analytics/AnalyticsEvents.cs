
public class ButtonSPressedEvent : Unity.Services.Analytics.Event
{
   public ButtonSPressedEvent() : base("buttonPressed")
   {

   }

    public int number { set { SetParameter("number", value); } }
    public string level_id { set { SetParameter("level_id", value); } }
    public string user_id { set { SetParameter("user_id", value); } }
}

