
//The EnumCollections just needs to be in the project, no GameObject needed
//Can be freely change to suit the project, calls need to be changed
namespace EnumCollection
{   
    public enum GameState
    {
        MainMenu,
        Credits,
        Settings,
        HighscoreMenu,
        Starting,
        GameOver,
        NewGame,
        Quit,
    }

    public enum Fade
    {
        In,
        Out,
    }

    //Tracks and SFX will be in alphabetical order depending on the name
    //in the Assets/Resources Folder
    public enum Track
    {
        MainMenu,
        GameTrackOne,
        GameTrackTwos,
    }

    public enum SFX
    {
        ButtonClick,
        PlayerGotHit,
    }
}