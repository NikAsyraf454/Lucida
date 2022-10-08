using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    bool showHelp;

    string input;

    public static DebugCommand KILL_ALL;
    public static DebugCommand ROSEBUD;
    public static DebugCommand<int> SET_GOLD;
    public static DebugCommand HELP;
    public static DebugCommand<int> DAMAGEPLAYER;

    public List<object> commandList;

    public void OnToggleDebug(InputValue value)
    {
        showConsole = !showConsole;
    }

    public void OnReturn(InputValue value)
    {
        if(showConsole)
        {
            HandleInput();
            input = "";
        }
    }

    private void Awake()
    {
        // KILL_ALL = new DebugCommand("kill_all", "Removes all enemys from the scene", "kill_all", () =>
        // {

        // });

        //set the name different, or it might execute multiple command
        
        ROSEBUD = new DebugCommand("rosebud", "Adds 100 gold.", "rosebud", () =>
        {
            PlayerManager.Instance.IncreaseResource(100);
        });

        SET_GOLD= new DebugCommand<int>("set_gold", "Sets amount of gold", "set_gold <gold_amount>", (x) =>
        {
            PlayerManager.Instance.IncreaseResource(x);
        });

        DAMAGEPLAYER = new DebugCommand<int>("damageplayer", "Sets amount of player to reduce", "damageplayer <damageamount>", (x) =>
        {
            PlayerManager.Instance.ReducePlayerHealth(x);
        });

        HELP = new DebugCommand("help", "Shows a list of commands", "help", () =>
        {
            showHelp = true;
        });

        commandList = new List<object>
        {
            ROSEBUD,
            SET_GOLD,
            DAMAGEPLAYER,
            HELP
        };


    }

    Vector2 scroll;

    private void OnGUI()
    {
        if(!showConsole) {return;}

        float y = 0f;//Screen.height-30;

        if(showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");
            
            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y+5f, Screen.width, 90), scroll, viewport);

            for(int i=0; i<commandList.Count; i++)
            {

                DebugCommandBase command = commandList[i] as DebugCommandBase;

                string label = $"{command.commandFormat} - {command.commandDescription}";

                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                GUI.Label(labelRect, label);
            } 

            GUI.EndScrollView();

            y += 100;

        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0,0,0,0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width -20f, 20f), input);
    }

    private void HandleInput()
    {
        string[] properties = input.Split(' ');

        for(int i = 0; i<commandList.Count; i++)
        {

            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if(input.Contains(commandBase.commandId))
            {

                if(commandList[i] as DebugCommand != null)
                {
                    //cast this type and invoke this command
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if(commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
            }
        }
    }
}
