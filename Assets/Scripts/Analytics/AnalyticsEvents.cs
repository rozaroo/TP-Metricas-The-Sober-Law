
public class ButtonSPressedEvent : Unity.Services.Analytics.Event
{
   public ButtonSPressedEvent() : base("ButtonSPressed")
   {

   }

    public int Num { set { SetParameter("Num", value); } }
    public string Level_ID { set { SetParameter("Level_ID", value); } }
    public string usuario_identified { set { SetParameter("usuario_identified", value); } }
}

public class instructionsClickedEvent : Unity.Services.Analytics.Event 
{
    public instructionsClickedEvent() : base("instructionsClicked") 
    {

    }
    public int Num { set { SetParameter("Num", value); } }
    public string usuario_identified { set { SetParameter("usuario_identified", value); } }
}

public class medkitPickedEvent : Unity.Services.Analytics.Event 
{
    public medkitPickedEvent() : base("medkitPickedUp") 
    {

    }
    public int Cant { set { SetParameter("Cant", value); } }
    public string Level_ID { set { SetParameter("Level_ID", value); } }
    public string usuario_identified { set { SetParameter("usuario_identified", value); } }
}

public class playerDeathsPerLevelEvent : Unity.Services.Analytics.Event 
{
    public playerDeathsPerLevelEvent() : base("playerDeathsPerLevel") 
    {
        
    }
    public int DeathsPerLevel { set { SetParameter("DeathsPerLevel", value); } }
    public string Enemy_ID { set { SetParameter("Enemy_ID", value); } }
    public string Level_ID { set { SetParameter("Level_ID", value); } }
    public string usuario_identified { set { SetParameter("usuario_identified", value); } }
}

public class gameFinishedEvent : Unity.Services.Analytics.Event
{
    public gameFinishedEvent() : base("gameFinished")
    {

    }
    public float PlayTime { set { SetParameter("PlayTime", value); } }
    public string usuario_identified { set { SetParameter("usuario_identified", value); } }
}

