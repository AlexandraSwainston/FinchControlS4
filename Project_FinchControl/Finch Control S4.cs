using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;
using System.Linq;

namespace Project_FinchControl
{
    public enum Command
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        GETTEMPERATURE,
        DONE
    }

    // *****************************************************
    //
    // Title: Finch Control - Menu Starter
    // Description: Starter solution with the helper methods,
    //              opening and closing screens, and the menu
    // Application Type: Console
    // Author: Swainston, Alexandra
    // Dated Created: 1/22/2020
    // Last Modified: 3/29/2020
    //
    // *****************************************************

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            DataDisplaySetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static (ConsoleColor foregroundColor, ConsoleColor backgroundColor) ReadThemeData()
        {
            string dataPath = @"Data/Data.txt";
            string[] themeColors;

            ConsoleColor foregroundColor;
            ConsoleColor backgroundColor;

            DisplayScreenHeader("Read Theme Data");

            themeColors = File.ReadAllLines(dataPath); 

            Enum.TryParse(themeColors[0], true, out foregroundColor);
            Enum.TryParse(themeColors[1], true, out backgroundColor);

            Console.WriteLine();
            Console.WriteLine($"Current Foreground Color: {themeColors[0]}");
            Console.WriteLine($"Current Background Color: {themeColors[1]}");

            DisplayContinuePrompt();
            return (foregroundColor, backgroundColor);
        }

        static void WriteThemeData(ConsoleColor foreground, ConsoleColor background)
        {
            string dataPath = @"Data/Theme.txt";

            File.WriteAllText(dataPath, foreground.ToString() + "\n");
            File.AppendAllText(dataPath, background.ToString());
        }

        static ConsoleColor GetConsoleColorFromUser(string property)
        {
            ConsoleColor consoleColor;
            bool validConsoleColor;

            do
            {
                Console.Write($"\tEnter a value for the {property}:");
                validConsoleColor = Enum.TryParse<ConsoleColor>(Console.ReadLine(), true, out consoleColor);

                if (!validConsoleColor)
                {
                    Console.WriteLine("\n\t***** It appears you did not provide a valid console color. Please try again. *****\n");
                }
                else
                {
                    validConsoleColor = true;
                }
            } while (!validConsoleColor);

            return consoleColor;
        }

        static void DataDisplaySetTheme()
        {
            (ConsoleColor foregroundColor, ConsoleColor backgroundColor) themeColors;
            bool themeChosen = false;

            //
            // set current theme from data
            //
            themeColors = ReadThemeData();
            Console.ForegroundColor = themeColors.foregroundColor;
            Console.BackgroundColor = themeColors.backgroundColor;
            Console.Clear();
            DisplayScreenHeader("Set Application Theme");

            Console.WriteLine($"\tCurrent Foreground Color: {Console.ForegroundColor}");
            Console.WriteLine($"\tCurrent Background Color: {Console.BackgroundColor}");
            Console.WriteLine();

            Console.Write("\tWould you like to change the current theme [ yes | no ]?");
            do
            {
                themeColors.foregroundColor = GetConsoleColorFromUser("Foreground");
                themeColors.backgroundColor = GetConsoleColorFromUser("Background");

                //
                // set new theme
                //
                Console.ForegroundColor = themeColors.foregroundColor;
                Console.BackgroundColor = themeColors.backgroundColor;
                Console.Clear();
                DisplayScreenHeader("Set Application Theme");

                Console.WriteLine($"\tCurrent Foreground Color: {Console.ForegroundColor}");
                Console.WriteLine($"\tCurrent Background Color: {Console.BackgroundColor}");
                Console.WriteLine();

                Console.WriteLine("\tIs this the theme you would like?");
                if (Console.ReadLine().ToLower() == "yes")
                {
                    themeChosen = true;
                    WriteThemeData(themeColors.foregroundColor, themeColors.backgroundColor);
                }

            } while (!themeChosen);  
            
            DisplayContinuePrompt();
        }
        

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Change Theme");
                Console.WriteLine("\tg) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":
                        DataRecorderMenuScreen(finchRobot);
                        break;

                    case "d":
                        LightAlarmDisplayMenuScreen(finchRobot);
                        break;

                    case "e":
                        UserProgrammingDisplayMenuScreen(finchRobot);
                        break;

                    case "f":
                        ChangeTheme();
                        break;

                    case "g":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        static void ChangeTheme()
        {
            ReadThemeData();
            DataDisplaySetTheme();
        }

        static void UserProgrammingDisplayMenuScreen(Finch finchRobot)
        {
            string menuChoice;
            bool quitMenu = false;

            //
            // tuple to store all three command parameters
            //
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            List<Command> commands = new List<Command>();





            do
            {
                DisplayScreenHeader("UserProgrammingMenu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        commandParameters = UserProgrammingDisplayGetCommandParameters();
                        break;

                    case "b":
                        UserProgrammingDisplayGetFinchCommands(commands);
                        break;

                    case "c":
                        UserProgrammingDisplayFinchCommands(commands);
                        break;

                    case "d":
                        UserProgrammingDisplayExecuteFinchCommmands(finchRobot, commands, commandParameters);
                        break;

                    case "q":
                        quitMenu = true;
                        break;
                }
            } while (true);
        }

        static void UserProgrammingDisplayExecuteFinchCommmands(
            Finch finchRobot, List<Command> commands, 
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters)
        {
            int motorSpeed = commandParameters.motorSpeed;
            int ledBrightness = commandParameters.ledBrightness;
            int waitMilliSeconds = (int)(commandParameters.waitSeconds * 1000);
            string commandFeedback = "";
            const int TURNING_MOTOR_SPEED = 100;

            DisplayScreenHeader("Execute Finch Commands");

            Console.WriteLine("\tThe Finch Robot is ready to execute the list of commands.");
            DisplayContinuePrompt();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        break;

                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        commandFeedback = Command.MOVEFORWARD.ToString();
                        break;

                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);
                        commandFeedback = Command.MOVEBACKWARD.ToString();
                        break;

                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        commandFeedback = Command.STOPMOTORS.ToString();
                        break;

                    case Command.WAIT:
                        finchRobot.wait(waitMilliSeconds);
                        commandFeedback = Command.WAIT.ToString();
                        break;

                    case Command.TURNRIGHT:
                        finchRobot.setMotors(TURNING_MOTOR_SPEED, -TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNRIGHT.ToString();
                        break;

                    case Command.TURNLEFT:
                        finchRobot.setMotors(-TURNING_MOTOR_SPEED, TURNING_MOTOR_SPEED);
                        commandFeedback = Command.TURNLEFT.ToString();
                        break;

                    case Command.LEDON:
                        finchRobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        commandFeedback = Command.LEDON.ToString();
                        break;

                    case Command.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        commandFeedback = Command.LEDOFF.ToString();
                        break;

                    case Command.GETTEMPERATURE:
                        commandFeedback = $"Temperature: {finchRobot.getTemperature().ToString("n2")}°C\n";
                        break;

                    case Command.DONE:
                        commandFeedback = Command.DONE.ToString();
                        break;

                    default:
                        break;
                }

                Console.WriteLine($"\t{commandFeedback}");
            }

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingDisplayFinchCommands(List<Command> commands)
        {
            DisplayScreenHeader("Finch Robot Commands");

            foreach (Command command in commands)
            {
                Console.WriteLine($"\t{command}");
            }

            DisplayMenuPrompt("User Programming");
        }

        static void UserProgrammingDisplayGetFinchCommands(List<Command> commands)
        {
            Command command = Command.NONE;

            DisplayScreenHeader("Finch Robot Commands");

            //
            // list commands
            //
            int commandCount = 1;
            Console.WriteLine("\tList of Available Commands");
            Console.WriteLine();
            Console.Write("\t-");
            foreach (string commandName in Enum.GetNames(typeof(Command)))
            {
                Console.Write($"- {commandName}  -");
                if (commandCount % 5 == 0) Console.Write("-\n\t-");
                commandCount++;
            }
            Console.WriteLine();

            while (command != Command.DONE)
            {
                Console.Write("\tEnter Command:");

                if (Enum.TryParse(Console.ReadLine().ToUpper(), out command))
                {
                    commands.Add(command);
                }
                else
                {
                    Console.WriteLine("\t\t*******************************************");
                    Console.WriteLine("\t\tPlease enter a command from the list above.");
                    Console.WriteLine("\t\t*******************************************");
                }
            }
        }

        static (int motorSpeed, int ledBrightness, double waitSeconds) UserProgrammingDisplayGetCommandParameters()
        {
            DisplayScreenHeader("Command Parameters");

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParameters;
            commandParameters.motorSpeed = 0;
            commandParameters.ledBrightness = 0;
            commandParameters.waitSeconds = 0;

            GetMotorSpeed("\tEnter Motor Speed [1 - 255]", 1, 255, out commandParameters.motorSpeed);
            GetLEDBrightness("\tEnter LED Brightness [1 - 255]:", 1, 255, out commandParameters.ledBrightness);
            GetWaitTime("\tEnter Wait in Seconds:", 0, 10, out commandParameters.waitSeconds);

            Console.WriteLine();
            Console.WriteLine($"\tMotor Speed: {commandParameters.motorSpeed}");
            Console.WriteLine($"\tLED Brightness: {commandParameters.ledBrightness}");
            Console.WriteLine($"\tWait Command Duration: {commandParameters.waitSeconds}");

            DisplayMenuPrompt("User Programming");

            return commandParameters;
        }


        static double GetWaitTime(string v1, int v2, int v3, out double waitSeconds)
        {
            do
            {
                DisplayScreenHeader("Wait Time");

                Console.WriteLine();
                Console.WriteLine("Please Enter Wait Time in Seconds: [1 - 10]");
                double.TryParse(Console.ReadLine(), out waitSeconds);

                if (waitSeconds < 1 || waitSeconds > 10)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid wait time between 1 and 10, as previously stated.");
                    DisplayContinuePrompt();
                }                
            } while (waitSeconds < 1 || waitSeconds > 10);

            return waitSeconds;
        }

        static int GetMotorSpeed(string v1, int v2, int v3, out int motorSpeed)
        {
            do
            {
                DisplayScreenHeader("Get Motor Speed");

                Console.WriteLine();
                Console.WriteLine("Please Enter Motor Speed: [1 - 255]");
                int.TryParse(Console.ReadLine(), out motorSpeed);

                if (motorSpeed < 1 || motorSpeed > 255)
                {
                    Console.WriteLine("Please enter a valid motor speed between 1 and 255, as previously stated.");
                    DisplayContinuePrompt();
                }
            } while (motorSpeed < 1 || motorSpeed > 255);

            return motorSpeed;
        }

        static int GetLEDBrightness(string v1, int v2, int v3, out int ledBrightness)
        {
            do
            {
                DisplayScreenHeader("LED Brightness");

                Console.WriteLine();
                Console.WriteLine("Please Enter LED Brightness: [1 - 255]");
                int.TryParse(Console.ReadLine(), out ledBrightness);

                if (ledBrightness < 1 || ledBrightness > 255)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a valid LED brightness between 1 and 255, as previously stated.");
                    DisplayContinuePrompt();
                }

            } while (ledBrightness < 1 || ledBrightness > 255);

            return ledBrightness;
        }

        #region ALARM SYSTEM

        static void LightAlarmDisplayMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            string sensorsToMonitor = "";
            string rangeType = "";
            int minMaxThresholdValue = 0;
            int timeToMonitor = 0;

            bool quitMenu = false;
            string menuChoice;
            do
            {
                DisplayScreenHeader("Data Recorder Menu");


                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors to Monitors");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Minimum/Maximum Threshold Value");
                Console.WriteLine("\td) Set Time to Monitor");
                Console.WriteLine("\te) Set Alarm");
                Console.WriteLine("\tq) Return to Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sensorsToMonitor = LightAlarmDisplaySetSensorsToMonitor();
                        break;

                    case "b":
                        rangeType = LightAlarmDisplaySetRangeType();
                        break;

                    case "c":
                        minMaxThresholdValue = LightAlarmDisplaySetMinimumMaximumThresholdValue(rangeType, finchRobot);
                        break;

                    case "d":
                        timeToMonitor = LightAlarmDisplaySetTimeToMonitor();
                        break;

                    case "e":
                        LightAlarmSetAlarm(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);
                        break;

                    case "q":
                        quitMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitMenu);
        }

        static void LightAlarmSetAlarm(Finch finchRobot, string sensorsToMonitor, string rangeType, int minMaxThresholdValue, int timeToMonitor)
        {
            bool thresholdExceeded = false;

            DisplayScreenHeader("Set Alarm");

            Console.WriteLine($"\tSensor(s) to Monitor: {sensorsToMonitor}");
            Console.WriteLine($"\tRange Type: {rangeType}");
            Console.WriteLine($"\t{rangeType} Threshold Value: {minMaxThresholdValue}");
            Console.WriteLine($"\tTime To Monitor: {timeToMonitor}");
            Console.WriteLine();

            Console.WriteLine("Press Any Key to Begin Monitoring.");
            Console.ReadKey();

            thresholdExceeded = LightAlarmMonitorLightSensors(finchRobot, sensorsToMonitor, rangeType, minMaxThresholdValue, timeToMonitor);

            if (thresholdExceeded)
            {
                Console.WriteLine($"The {rangeType} threshold value of {minMaxThresholdValue} was exceeded.");
            }
            else
            {
                Console.WriteLine("The threshold value was not exceeded.");
            }

            DisplayMenuPrompt("Light Alarm");
        }

        static void LightAlarmDisplayElapsedTime(int elapsedTime)
        {
            Console.SetCursorPosition(15, 10);
            Console.WriteLine($"Elapsed Time:              ");
            Console.WriteLine($"Elapsed Time: {elapsedTime}");
        }

        static bool LightAlarmMonitorLightSensors(Finch finchRobot, string sensorstoMonitor, string rangeType, int minMaxThresholdValue, int timeToMonitor)
        {
            bool thresholdExceeded = false;
            int elapsedTime = 0;
            int currentLightSensorValue = 0;
            string sensorsToMonitor = "";

            while (!thresholdExceeded && elapsedTime < timeToMonitor)
            {
                currentLightSensorValue = LightAlarmGetCurrentLightSensorValue(finchRobot, sensorsToMonitor);

                switch (rangeType)
                {
                    case "minimum or maximum":
                        if (currentLightSensorValue < minMaxThresholdValue || currentLightSensorValue > minMaxThresholdValue)
                        {
                            thresholdExceeded = true;
                            Console.Beep();
                        }
                        break;
                }

                finchRobot.wait(1000);
                elapsedTime++;
                LightAlarmDisplayElapsedTime(elapsedTime);
            }

            return thresholdExceeded;
        }

        static int LightAlarmGetCurrentLightSensorValue(Finch finchRobot, string sensorsToMonitor)
        {
            int currentLightSensorValue = 0;

            switch (sensorsToMonitor)
            {
                case "left":
                    currentLightSensorValue = finchRobot.getLeftLightSensor();
                    break;

                case "right":
                    currentLightSensorValue = finchRobot.getRightLightSensor();
                    break;

                case "both":
                    currentLightSensorValue = (finchRobot.getLeftLightSensor() + finchRobot.getRightLightSensor()) / 2;
                    currentLightSensorValue = (int)(finchRobot.getLightSensors().Average());
                    break;
            }
            return currentLightSensorValue;
        }

        static int LightAlarmDisplaySetTimeToMonitor()
        {
            int timeToMonitor;
            bool validResponse;
               
            do
            {
                DisplayScreenHeader("Time to Monitor");

                Console.Write("Time to Monitor [seconds]:");
                validResponse = int.TryParse(Console.ReadLine(), out timeToMonitor);

                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a number.");
                    DisplayContinuePrompt();
                }
            } while (!validResponse);
            
            DisplayContinuePrompt();           
            
            return timeToMonitor; 
            


            
        }

        static int LightAlarmDisplaySetMinimumMaximumThresholdValue(string rangeType, Finch finchRobot)
        {
            int minMaxThresholdValue;
            bool validResponse;

            do
            {
            DisplayScreenHeader("Min/Max Threshold Value");

            Console.WriteLine($"Left Light Sensor: {finchRobot.getLeftLightSensor()}");
            Console.WriteLine($"Current Right Light Sensor: {finchRobot.getRightLightSensor()}");
            Console.WriteLine();

            Console.Write($"{rangeType} Light Sensor Value: ");
            validResponse = int.TryParse(Console.ReadLine(), out minMaxThresholdValue);

                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a number.");
                    DisplayContinuePrompt();
                }
            } while (!validResponse);
            DisplayContinuePrompt();

            return minMaxThresholdValue;
        }

        static string LightAlarmDisplaySetRangeType()
        {
            string rangeType;
            string userResponse;

            do
            {
                DisplayScreenHeader("Range Type");

                Console.Write("Range Type: [minimum, maximum]");
                userResponse = Console.ReadLine().ToLower();
                rangeType = userResponse;

                if (userResponse != "minimum" && userResponse != "maximum")
                {
                    Console.WriteLine("Please enter 'minimum' or 'maximum'");
                    DisplayContinuePrompt();
                }
            } while (rangeType != "minimum" && rangeType != "maximum");

            DisplayContinuePrompt();

            return rangeType;
        }

        static string LightAlarmDisplaySetSensorsToMonitor()
        {
            string sensorsToMonitor;
            string userResponse;

            do
            {
                DisplayScreenHeader("Sensors to Monitor");

                Console.Write("Sensors to Monitor: [right, left, both]");
                userResponse = Console.ReadLine().ToLower();
                sensorsToMonitor = userResponse;

                if (userResponse != "left" && userResponse != "right" && userResponse != "both")
                {
                    Console.WriteLine("Please enter 'left', 'right', or 'both'");
                    DisplayContinuePrompt();
                }
            } while (sensorsToMonitor != "left" && sensorsToMonitor != "right" && sensorsToMonitor != "both");

            DisplayContinuePrompt();

            return sensorsToMonitor;
        }
        #endregion

        #region DATA RECORDER

        static void DataRecorderMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;

            int numberOfPoints = 0;
            double dataPointFrequency = 0;
            double[] fahrenheit = null;

            bool quitDataRecorderMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\tq) Return to Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                       numberOfPoints = DataRecorderDisplayGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayGetDataPointFrequency();
                        break;

                    case "c":
                        fahrenheit = new double[numberOfPoints];
                        fahrenheit = DataRecorderDisplayGetData(numberOfPoints, dataPointFrequency, finchRobot, fahrenheit);                       
                        break;

                    case "d":
                        DataRecorderDisplayTable(fahrenheit);
                        break;

                    case "q":
                        quitDataRecorderMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitDataRecorderMenu);
        }

        static void DataRecorderDisplayTable(double[] temperatures)
        {
            //
            // table headers
            //
            Console.WriteLine(
                "Data Point".PadLeft(12) +
                "Temp".PadLeft(10)
                );
            Console.WriteLine(
                "----------".PadLeft(12) +
                "----".PadLeft(10)
                );

            //
            // table data
            //
            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine(
                (index + 1).ToString().PadLeft(12) +
                temperatures[index].ToString("n2").PadLeft(10)
                );

            }
            DisplayContinuePrompt();
        }

        static double[] DataRecorderDisplayGetData(int numberOfPoints, double dataPointFrequency, Finch finchRobot, double[] fahrenheit)
        {
            double temperatures;
            int frequencyInSeconds;

            DisplayScreenHeader("Get Data");

            Console.WriteLine();
            Console.WriteLine($"Data Points: {numberOfPoints} and Data Point Frequency: {dataPointFrequency}");

            Console.WriteLine("The Finch Robot is ready to record temperatures.");
            DisplayContinuePrompt();

            for (int index = 0; index < numberOfPoints; index++)
            {
                temperatures = finchRobot.getTemperature();
                fahrenheit[index] = ConvertCelsiusToFahrenheit(temperatures);
                Console.WriteLine($"Data #{index + 1}: {fahrenheit[index]} °F");
                frequencyInSeconds = (int)(dataPointFrequency * 1000);
                finchRobot.wait(frequencyInSeconds);
            }


            Console.WriteLine();
            Console.WriteLine("Current Data");
            DataRecorderDisplayTable(fahrenheit);

            Console.WriteLine();
            Console.WriteLine($"Average Temperature: {fahrenheit.Average()}");

            DisplayContinuePrompt();

            return fahrenheit;
        }

        static double ConvertCelsiusToFahrenheit(double temperatures)
        {
            double fahrenheit;
            double temperatureF = ((temperatures * 1.8) + 32);

            fahrenheit = temperatureF;

            return fahrenheit;
        }

        static double DataRecorderDisplayGetDataPointFrequency()
        {
            double dataPointFrequency;
            bool validResponse;

            do
            {
            DisplayScreenHeader("Data Point Frequency");

            Console.Write("Data Point Frequency:");          
            
            validResponse = double.TryParse(Console.ReadLine(), out dataPointFrequency);

            Console.WriteLine();
            Console.WriteLine($"Data Point Frequency: {dataPointFrequency}");
            
                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a number.");
                    DisplayContinuePrompt();
                }            
            } while (!validResponse);
            
            DisplayContinuePrompt();
            return dataPointFrequency;
            
        }

        static int DataRecorderDisplayGetNumberOfDataPoints()
        {
            int numberOfDataPoints;
            bool validResponse;

            do
            {
            DisplayScreenHeader("Number of Data Points");

            Console.Write("Number of Data Points:");

            validResponse = int.TryParse(Console.ReadLine(), out numberOfDataPoints);

            Console.WriteLine();
            Console.WriteLine($"Number of Data Points: {numberOfDataPoints}");

                if (!validResponse)
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter a number.");
                    DisplayContinuePrompt();
                }
            
            } while (!validResponse);  
            
            DisplayContinuePrompt();
            return numberOfDataPoints;
            
        }


        #endregion

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch myFinch)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Dance");
                Console.WriteLine("\tc) Mixing It Up");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLightAndSound(myFinch);
                        break;

                    case "b":
                        DisplayDance(myFinch);
                        break;

                    case "c":
                        DisplayMixingItUp(myFinch);
                        break;

                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        static void DisplayMixingItUp(Finch finchRobot)
        {
            DisplayScreenHeader("Mixing It Up");

            for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
            {
                finchRobot.setLED(255, 0, 0);
                finchRobot.wait(500);
                finchRobot.setLED(0, 0, 0);
                finchRobot.setLED(255, 0, 255);
                finchRobot.wait(500);
                finchRobot.setLED(255, 0, 255);
                finchRobot.wait(500);
                finchRobot.setLED(0, 255, 255);
                finchRobot.wait(500);
                finchRobot.setLED(0, 255, 0);
                finchRobot.wait(500);
                finchRobot.setLED(0, 0, 255);

                finchRobot.noteOn(lightSoundLevel * 100);
                finchRobot.wait(500);

                finchRobot.setMotors(255, 255);
                finchRobot.wait(500);
                finchRobot.setMotors(0, 0);
                finchRobot.setMotors(-255, 255);
                finchRobot.wait(1000);
                finchRobot.setMotors(0, 0);
            }

            DisplayContinuePrompt();
        }

        static void DisplayDance(Finch myFinch)
        {
            DisplayScreenHeader("Finchy Moves");

            myFinch.setMotors(255, 255);
            myFinch.wait(500);
            myFinch.setMotors(0, 0);
            myFinch.setMotors(-255, 255);
            myFinch.wait(1000);
            myFinch.setMotors(0, 0);

            DisplayContinuePrompt();

        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayLightAndSound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will now show off its glowing talent!");
            DisplayContinuePrompt();

            for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
            {
                finchRobot.setLED(255, 0, 0);
                finchRobot.wait(500);
                finchRobot.setLED(0, 0, 0);
                finchRobot.setLED(255, 0, 255);
                finchRobot.wait(500);
                finchRobot.setLED(255, 0, 255);
                finchRobot.wait(500);
                finchRobot.setLED(0, 255, 255);
                finchRobot.wait(500);
                finchRobot.setLED(0, 255, 0);
                finchRobot.wait(500);
                finchRobot.setLED(0, 0, 255);

                finchRobot.noteOn(lightSoundLevel * 100);
                finchRobot.wait(500);

            }

            DisplayMenuPrompt("Talent Show Menu");
        }

        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
    {
        Console.CursorVisible = false;

        DisplayScreenHeader("Disconnect Finch Robot");

        Console.WriteLine("\tAbout to disconnect from the Finch robot.");
        DisplayContinuePrompt();

        finchRobot.disConnect();

        Console.WriteLine("\tThe Finch robot is now disconnect.");

        DisplayMenuPrompt("Main Menu");
    }

    /// <summary>
    /// *****************************************************************
    /// *                  Connect the Finch Robot                      *
    /// *****************************************************************
    /// </summary>
    /// <param name="finchRobot">finch robot object</param>
    /// <returns>notify if the robot is connected</returns>
    static bool DisplayConnectFinchRobot(Finch finchRobot)
    {
        Console.CursorVisible = false;

        bool robotConnected;

        DisplayScreenHeader("Connect Finch Robot");

        Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
        DisplayContinuePrompt();

        robotConnected = finchRobot.connect();

        // TODO test connection and provide user feedback - text, lights, sounds

        DisplayMenuPrompt("Main Menu");

        //
        // reset finch robot
        //
        finchRobot.setLED(0, 0, 0);
        finchRobot.noteOff();

        return robotConnected;
    }

    #endregion

    #region USER INTERFACE

    /// <summary>
    /// *****************************************************************
    /// *                     Welcome Screen                            *
    /// *****************************************************************
    /// </summary>
    static void DisplayWelcomeScreen()
    {
        Console.CursorVisible = false;

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("\t\tFinch Control");
        Console.WriteLine();

        DisplayContinuePrompt();
    }

    /// <summary>
    /// *****************************************************************
    /// *                     Closing Screen                            *
    /// *****************************************************************
    /// </summary>
    static void DisplayClosingScreen()
    {
        Console.CursorVisible = false;

        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("\t\tThank you for using Finch Control!");
        Console.WriteLine();

        DisplayContinuePrompt();
    }

    /// <summary>
    /// display continue prompt
    /// </summary>
    static void DisplayContinuePrompt()
    {
        Console.WriteLine();
        Console.WriteLine("\tPress any key to continue.");
        Console.ReadKey();
    }

    /// <summary>
    /// display menu prompt
    /// </summary>
    static void DisplayMenuPrompt(string menuName)
    {
        Console.WriteLine();
        Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
        Console.ReadKey();
    }

    /// <summary>
    /// display screen header
    /// </summary>
    static void DisplayScreenHeader(string headerText)
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine("\t\t" + headerText);
        Console.WriteLine();
    }

    #endregion
}
}
